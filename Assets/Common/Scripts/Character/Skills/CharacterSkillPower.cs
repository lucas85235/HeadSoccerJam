using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MadeInHouse.Powers;

namespace MadeInHouse.Characters
{
    public class CharacterSkillPower : CharacterSkill
    {
        protected Transform canvas;
        protected CharacterPower power;

        [Header("Setup")]
        public Slider powerSlider;
        public float maxPower = 100f;

        [Header("Increment")]
        public float bySecond = 0.1f;
        public float whenScoring = 5;
        public float whenTakingGoal = 10;
        public float whenInteract = 2;

        protected override IEnumerator Start()
        {
            base.Start();

            yield return new WaitUntil( () => character.input != null );

            InputCode = character.input.Power;

        }

        protected override void Initialize()
        {
            base.Initialize();

            power = GetComponent<CharacterPower>();
            canvas = GameObject.Find("PlayerHud").transform;
            powerSlider = Instantiate(powerSlider, canvas);
            powerSlider.maxValue = maxPower;
            powerSlider.value = 0;
        }

        public override void UseSkill()
        {
            base.UseSkill();

            if (!canUseSkills) return;

            if (powerSlider.value == powerSlider.maxValue)
            {
                powerSlider.value = 0;

                if (power != null)
                {
                    power.UsePower();
                }
            }
        }

        public virtual void Increment(IncrementType i)
        {
            float iPower = 0;

            if (IncrementType.second == i)
                iPower = bySecond * Time.deltaTime;

            else if (IncrementType.scoring == i)
                iPower = whenScoring;

            else if (IncrementType.taking == i)
                iPower = whenTakingGoal;

            else if (IncrementType.interact == i)
                iPower = whenInteract;

            powerSlider.value += iPower;
        }
    }

    public enum IncrementType
    {
        second,
        scoring,
        taking,
        interact
    }
}
