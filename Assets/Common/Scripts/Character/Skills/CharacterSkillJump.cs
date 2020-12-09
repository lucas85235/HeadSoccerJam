using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillJump : CharacterSkill
    {
        protected Rigidbody rb;

        protected bool isGround;
        protected float distToGround;

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
            distToGround = GetComponent<Collider>().bounds.extents.y;
            rb = GetComponent<Rigidbody>();
        }

        /// <summary> Add force to a rigidbody for jump if detect ground </summary>
        public override void Skill()
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        /// <summary> Detect contact with ground </summary>
        public virtual bool IsGrounded()
        {
            isGround = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
            return isGround;
        }
    } 
}
