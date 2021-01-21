using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse.Characters;

namespace MadeInHouse.Powers
{
    public class PowerBurst : CharacterPower
    {
        protected CharacterSkillMove move;
        protected CharacterSkillJump jump;
        protected CharacterSkillKick kick;
        protected CharacterSkillHeadButt head;
        protected CharacterSkillLife life;

        protected float tempMoveSpeed;
        protected float tempJumpForce;
        protected float tempKickForce;
        protected float tempButtForce;

        protected bool inUse;
        protected float amountTimer;
        protected float countTimer;

        [Header("Setup")]
        public float powerTimer = 5f;
        [Range(0f, 1f)] public float incrementPorcent = 0.3f;

        protected override void Initialize()
        {
            base.Initialize();

            move = GetComponent<CharacterSkillMove>();
            jump = GetComponent<CharacterSkillJump>();
            kick = GetComponent<CharacterSkillKick>();
            head = GetComponent<CharacterSkillHeadButt>();
            life = GetComponent<CharacterSkillLife>();

            tempMoveSpeed = move.moveSpeed;
            tempJumpForce = jump.jumpForce;
            tempKickForce = kick.kickForce;
            tempButtForce = head.buttForce;
            
            amountTimer = powerTimer;
            inUse = false;
        }

        protected virtual void FixedUpdate() 
        {
            if (inUse)
            {
                if (countTimer > amountTimer)
                {
                    inUse = false;
                    UsePowerFinish();
                }
                else countTimer += Time.fixedDeltaTime;
            }
        }

        public override void UsePower()
        {
            if (!inUse)
            {
                inUse = true;

                life.SetMaxLife();

                move.moveSpeed = move.moveSpeed * (1 + incrementPorcent);
                jump.jumpForce = jump.jumpForce * (1 + incrementPorcent);
                kick.kickForce = kick.kickForce * (1 + incrementPorcent);
                head.buttForce = head.buttForce * (1 + incrementPorcent);

                countTimer = 0;
            }

            else amountTimer += powerTimer;
        }

        protected virtual void UsePowerFinish()
        {
            move.moveSpeed = tempMoveSpeed;
            jump.jumpForce = tempJumpForce;
            kick.kickForce = tempKickForce;
            head.buttForce = tempButtForce;

            amountTimer = powerTimer;
        }
    }
}
