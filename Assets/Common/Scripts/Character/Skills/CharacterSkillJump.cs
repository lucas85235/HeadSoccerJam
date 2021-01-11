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

        [Header("Stats")]
        [Range(30, 60)] public float jumpForce = 40;

        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Jump;
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
            if (detectGround.IsGrounded())
            {
                if (anim != null)
                {
                    anim.SetTrigger("Jump");
                }
                
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    } 
}
