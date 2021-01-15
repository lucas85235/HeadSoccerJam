using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInHouse.Characters
{
    public class CharacterSkillLife : CharacterSkill
    {
        protected Character character;

        [Header("Life Setup")]
        public Slider lifeSlider;
        public int maxLife = 5;

        [Header("Knockout Setup")]
        public float knockoutTimer = 1.5f;
        public float invencibleTimer = 0.5f;

        protected bool isKnockout = false;

        protected override void Initialize()
        {
            base.Initialize();

            character = GetComponent<Character>();

            var canvas = GameObject.Find("PlayerHud").transform;
            lifeSlider = Instantiate(lifeSlider, canvas);
            lifeSlider.maxValue = maxLife;
            lifeSlider.value = maxLife;
        }

        public override void UseSkill()
        {
            base.UseSkill();
            if (isKnockout == true) return;

            lifeSlider.value -= 1;

            if (lifeSlider.value < 1)
            {
                Knockout();
            }
        }

        protected virtual void Knockout()
        {
            isKnockout = true;
            character.SetCanUseSkills(false);
            Invoke("Revive", knockoutTimer);
        }

        protected virtual void Revive()
        {
            lifeSlider.value = maxLife;
            character.SetCanUseSkills(true);
            Invoke("ResetKnockout", invencibleTimer);
        }

        protected virtual void ResetKnockout()
        {
            isKnockout = false;
        }
    }
}
