using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse.Characters;

namespace MadeInHouse.Powers
{
    public class PowerStum : CharacterPower
    {
        protected CharacterSkillLife characterLife;
        
        protected override void Initialize()
        {
            base.Initialize();
            characterLife = otherCharacter.GetComponent<CharacterSkillLife>();
        }

        public override void UsePower()
        {
            if (characterLife != null)
            {
                if (characterLife.isKnockout)
                {
                    characterLife.ExtendKnockTimer();
                }
                else characterLife.DecreaseLife(characterLife.maxLife);
            }
        }
    }
}
