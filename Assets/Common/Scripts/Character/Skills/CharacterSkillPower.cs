﻿using System.Collections;
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
        public int incrementValue = 1;

        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Power;
        }

        protected override void Initialize()
        {
            base.Initialize();

            power = GetComponent<CharacterPower>();
            canvas = GameObject.Find("PlayerHud").transform;
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

                if (power != null)
                {
                    power.UsePower();
                }
            }
        }

        public virtual void Increment()
        {
            powerSlider.value += incrementValue;
        }
    }
}
