﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillMove : CharacterSkill
    {
        protected Vector3 moveInput;

        [Header("Stats")]
        [Range(4, 10)] public float moveSpeed = 7;
        [Range(2, 4)] public float animSmooth = 3;

        protected override IEnumerator Start()
        {
            base.Start();
            
            yield return new WaitUntil( () => character.input != null );

            InputAxis = character.input.AxisX;
        }

        public override void InputHandle()
        {
            base.InputHandle();

            if (!canUseSkills)
            {
                if (anim != null)
                {
                    anim.SetFloat("Speed", 0);
                }
                return;
            }

            if (character.input != null)
            {
                moveInput = new Vector3(character.input.AxisX(), 0);
                
                var speed = moveInput.x < 1 ? moveInput.x * animSmooth : moveInput.x;
                anim.SetFloat("Speed", speed);                
            }
        }

        public override void UseSkill()
        {
            base.UseSkill();
            transform.position += moveInput * moveSpeed * Time.deltaTime;
        }
    }
}
