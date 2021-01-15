using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillMove : CharacterSkill
    {
        protected Vector3 moveInput;

        [Header("Stats")]
        [Range(4, 10)] public float moveSpeed = 7;

        protected override void Start()
        {
            base.Start();
            InputAxis = InputSystem.Instance.AxisX;
        }

        public override void InputHandle()
        {
            base.InputHandle();
            moveInput = new Vector3(InputSystem.Instance.AxisX(), 0);

            if (moveInput.x != 0)
            {
                anim.SetBool("Move", true);
            }
            else
            {
                anim.SetBool("Move", false);
            }
        }

        public override void UseSkill()
        {
            base.UseSkill();

            if (!canUseSkills) return;

            transform.position += moveInput * moveSpeed * Time.deltaTime;
        }
    }
}
