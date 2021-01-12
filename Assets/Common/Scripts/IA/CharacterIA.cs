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
        protected BallBehaviour ball;
        protected DetectGround detectGround;
        protected CharacterSkillHeadButt skillHeadButt;
        protected CharacterSkillJump skillJump;

        protected bool canJump = true;
        protected float adjustingPosition;
        protected bool canMove = true;

        [Header("AI Difficulty Levels")]
        public DifficultyLevels cpuLevel = DifficultyLevels.easy;

        [Header("Private AI Parameters")]
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float minJumpDistance;
        [SerializeField] protected float jumpDelay;
        [SerializeField] protected float maxProtectedDistance;

        [Header("Move Rules")]
        public Vector2 cpuFieldLimits = new Vector2(-1f, 8f);

        protected virtual void Start()
        {
            anim = GetComponent<Animator>();
            ball = FindObjectOfType<BallBehaviour>();
            detectGround = FindObjectOfType<DetectGround>();

            skillHeadButt = GetComponent<CharacterSkillHeadButt>();
            skillJump = GetComponent<CharacterSkillJump>();

            SetCpuLevel();
        }

        protected void SetJumpSettings(float force, float distance, float delay)
        {
            skillJump.jumpForce = force;
            minJumpDistance = distance;
            jumpDelay = delay;
        }

        // Change the AI configuration based on the enum selection
        protected virtual void SetCpuLevel()
        {
            switch (cpuLevel)
            {
                case DifficultyLevels.easy:
                    moveSpeed = 6.0f;
                    maxProtectedDistance = 2.8f;
                    skillHeadButt.buttForce = 180;
                    SetJumpSettings(40f, 2.5f, 0.3f);
                    break;

                case DifficultyLevels.normal:
                    moveSpeed = 8.0f;
                    maxProtectedDistance = 3.4f;
                    skillHeadButt.buttForce = 220;
                    SetJumpSettings(50f, 3.5f, 0.2f);
                    break;

                case DifficultyLevels.hard:
                    moveSpeed = 10.0f;
                    maxProtectedDistance = 4.4f;
                    skillHeadButt.buttForce = 260;
                    SetJumpSettings(60f, 3.5f, 0.1f);
                    break;

                case DifficultyLevels.veryHard:
                    moveSpeed = 13.0f;
                    maxProtectedDistance = 5f;
                    skillHeadButt.buttForce = 280;
                    SetJumpSettings(60f, 3.5f, 0.05f);
                    break;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (canMove)
            {
                CpuMoveToBall();
            }
        }

        /// <summary> Main cpu play routines </summary>
        protected virtual void CpuMoveToBall()
        {
            if (ball.transform.position.x > cpuFieldLimits.x && ball.transform.position.x < cpuFieldLimits.y)
            {
                //move the cpu towards the ball

                var smoothStep = Mathf.SmoothStep(
                    transform.position.x, 
                    ball.transform.position.x + adjustingPosition, 
                    Time.fixedDeltaTime * moveSpeed);

                transform.position = new Vector3(
                    smoothStep,
                    transform.position.y,
                    transform.position.z);
            }

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

        /// <summary> Shoot the ball upon collision </summary>
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ball")
            {
                anim.SetTrigger("Kick");
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