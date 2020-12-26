using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillPower : CharacterSkill
    {
        protected Transform canvas;

        [Header("Setup")]
        public Slider powerSlider;
        public int incrementValue = 1;

        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Power;
        }

        protected override void Initialize()
        {
            base.Initialize();

            canvas = GameObject.Find("PlayerHud").transform;
            var tempPos = powerSlider.GetComponent<RectTransform>().localPosition;
            powerSlider = Instantiate(powerSlider, canvas);
            powerSlider.maxValue = 100;
            powerSlider.value = 0;
        }

        public override void Skill()
        {
            base.Skill();

            if (powerSlider.value == powerSlider.maxValue)
            {
                powerSlider.value = 0;
            }
        }

        public virtual void Increment()
        {
            powerSlider.value += incrementValue;
        }
    }
}
