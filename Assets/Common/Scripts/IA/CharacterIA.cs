using UnityEngine;
using System.Collections;

namespace MadeInHouse.Characters
{
    public class CharacterIA : MonoBehaviour
    {
        /// <summary>
        /// Main CPU controller. 
        /// This class ia a compact AI manager which moves the cpu opponent and manage its decisions.
        /// </summary>

        protected Animator anim;
        protected Rigidbody rb;
        protected BallBehaviour ball;
        protected DetectGround detectGround;
        protected CharacterSkill characterSkill;
        protected CharacterSkillHeadButt skillHeadButt;
        protected CharacterSkillKick skillKick;
        protected CharacterSkillJump skillJump;

        protected bool canJump = true;
        protected bool canAttack = true;
        public Vector3 lastPosition;

        [Header("AI Difficulty Levels")]
        public DifficultyLevels cpuLevel = DifficultyLevels.easy;

        [Header("Private AI Parameters")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float minJumpDistance;
        [SerializeField] protected float jumpSpeed;
        [SerializeField] protected float jumpDelay;
        [SerializeField] protected float maxProtectedDistance;
        [SerializeField] protected float attackCoolDown = 0.2f;

        [Header("Move Rules")]
        public Vector2 cpuFieldLimits = new Vector2(-2f, 9f);

        public bool canMove { get; set; }

        protected virtual void Awake()
        {
            anim = GetComponent<Character>().model.GetComponent<Animator>();
            
            rb = GetComponent<Rigidbody>();
            detectGround = GetComponent<DetectGround>();

            characterSkill = GetComponent<CharacterSkill>();
            skillHeadButt = GetComponent<CharacterSkillHeadButt>();
            skillKick = GetComponent<CharacterSkillKick>();
            skillJump = GetComponent<CharacterSkillJump>();

            canMove = true;
            lastPosition = transform.position;
        }

        void Start()
        {
            ball = FindObjectOfType<BallBehaviour>();
        }

        protected void SetJumpSettings(float force, float distance, float delay)
        {
            jumpSpeed = force;
            minJumpDistance = distance;
            jumpDelay = delay;
        }

        // Change the AI configuration based on the enum selection
        public virtual void SetCpuLevel(DifficultyLevels level)
        {
            cpuLevel = level;
            
            switch (level)
            {
                case DifficultyLevels.easy:
                    moveSpeed = 6.0f;
                    maxProtectedDistance = 2.8f;
                    skillHeadButt.buttForce = 180;
                    attackCoolDown = 0.6f;
                    SetJumpSettings(40f, 2.5f, 0.3f);
                    break;

                case DifficultyLevels.normal:
                    moveSpeed = 8.0f;
                    maxProtectedDistance = 3.4f;
                    skillHeadButt.buttForce = 220;
                    attackCoolDown = 0.5f;
                    SetJumpSettings(50f, 3.5f, 0.2f);
                    break;

                case DifficultyLevels.hard:
                    moveSpeed = 10.0f;
                    maxProtectedDistance = 4.4f;
                    skillHeadButt.buttForce = 260;
                    attackCoolDown = 0.4f;
                    SetJumpSettings(60f, 3.5f, 0.1f);
                    break;

                case DifficultyLevels.veryHard:
                    moveSpeed = 13.0f;
                    maxProtectedDistance = 5f;
                    skillHeadButt.buttForce = 280;
                    attackCoolDown = 0.3f;
                    SetJumpSettings(60f, 3.5f, 0.05f);
                    break;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (canMove)
            {
                if (!characterSkill.canUseSkills)
                {
                    return;
                }

                CpuMoveToBall();
                CpuAttackPlayer();
            }
        }

        /// <summary> Cpu attack player </summary>
        protected virtual void CpuAttackPlayer()
        {
            if (skillKick.playerCollision.isDetected && canAttack)
            {
                canAttack = false;
                skillKick.UseSkill();
                Invoke("AttackCoolDown", attackCoolDown);
            }
        }

        /// <summary> Time between attacks </summary>
        protected virtual void AttackCoolDown()
        {
            canAttack = true;
        }

        /// <summary> Main cpu play routines </summary>
        protected virtual void CpuMoveToBall()
        {
            if (!characterSkill.canUseSkills) return;

            if (ball.transform.position.x > cpuFieldLimits.x && ball.transform.position.x < cpuFieldLimits.y)
            {
                //move the cpu towards the ball
                var smoothStep = Mathf.SmoothStep(
                    transform.position.x,
                    ball.transform.position.x + 0.5f,
                    Time.fixedDeltaTime * moveSpeed);

                transform.position = new Vector3(
                    smoothStep,
                    transform.position.y,
                    transform.position.z);

                //if cpu is close enough to the ball, make it jump
                var ballDistance = Vector3.Distance(transform.position, ball.transform.position);

                if(ball.transform.position.y > 1f && ballDistance < minJumpDistance && detectGround.IsGrounded() && canJump) 
                {
                    canJump = false;
                    anim.SetTrigger("Jump");
                    StartCoroutine(JumpActivation());
                }

                // set move animation
                if (transform.position.x < lastPosition.x)
                {
                    anim.SetFloat("Speed", smoothStep);
                }
                else anim.SetFloat("Speed", -smoothStep);

                lastPosition = transform.position;
            }
            else anim.SetFloat("Speed", 0);

            if (ball.transform.position.x < cpuFieldLimits.x)
            {
                var smoothStep = Mathf.SmoothStep(
                    transform.position.x,
                    maxProtectedDistance,
                    Time.fixedDeltaTime * moveSpeed);

                transform.position = new Vector3(
                    smoothStep,
                    transform.position.y,
                    transform.position.z);
            }
        }

        /// <summary> enable jump ability again  </summary>
        private IEnumerator JumpActivation() 
        {
            yield return new WaitForSeconds(jumpDelay);
            Vector3 jumpPower = new Vector3(0, jumpSpeed - Random.Range(0, 20), 0);
            rb.AddForce(jumpPower, ForceMode.Impulse);
            anim.SetTrigger("JumpExit");

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil( () => detectGround.IsGrounded() );
            canJump = true;
        }

        /// <summary> Shoot the ball upon collision </summary>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (!characterSkill.canUseSkills) return;

            if (other.gameObject.tag == "Ball")
            {
                if (detectGround.IsGrounded()) anim.SetTrigger("Kick");
            }
        }

        // The different AI levels uses different parameters for the speed and accuracy of the cpu.
        public enum DifficultyLevels
        {
            easy,
            normal,
            hard,
            veryHard
        }
    }
}
