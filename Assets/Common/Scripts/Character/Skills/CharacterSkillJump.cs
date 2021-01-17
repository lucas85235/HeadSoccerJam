using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    [RequireComponent(typeof(DetectGround))]

    public class CharacterSkillJump : CharacterSkill
    {
        protected Rigidbody rb;
        protected DetectGround detectGround;

        protected bool canJump = true;
        protected bool inJump = false;

        [Header("Stats")]
        [Range(30, 60)] public float jumpForce = 40;
        public float jumpDelay = 0.1f;

        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Jump;
        }

        protected virtual void FixedUpdate()
        {
            if (detectGround.IsGrounded() && inJump)
            {
                inJump = false;

                if (anim != null)
                {
                    anim.SetTrigger("JumpExit");
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            rb = GetComponent<Rigidbody>();
            detectGround = GetComponent<DetectGround>();
        }

        /// <summary> Add force to a rigidbody for jump if detect ground </summary>
        public override void UseSkill()
        {
            base.UseSkill();

            if (!canUseSkills) return;

            if (detectGround.IsGrounded() && !inJump)
            {
                canJump = false;
                inJump = true;

                if (anim != null)
                {
                    anim.SetTrigger("Jump");
                }    

                Invoke("StartJump", jumpDelay);
            }
        }

        protected virtual void StartJump()
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
