﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    [RequireComponent(typeof(Character))]

    public class CharacterSkill : MonoBehaviour
    {
        protected Animator anim;
        protected Character character;
        protected CharacterIA characterIA;
        protected CharacterSkillPower skillPower;
        
        // if you want disable all skills use SetCanUseSkills in Character script
        [HideInInspector] public bool canUseSkills = true;

        [Header("Feedbakcs")]
        public AudioClip skillSound;

        private void Awake() 
        {
            Initialize();
        }

        protected virtual IEnumerator Start()
        {
            yield return null;
        }

        protected virtual void Initialize()
        {
            character = GetComponent<Character>();
            skillPower = GetComponent<CharacterSkillPower>();
            characterIA = GetComponent<CharacterIA>();
            anim = character.model.GetComponent<Animator>();
        }

        #region Input Handle

        public Func<float> InputAxis;
        public Func<bool> InputCode;

        public virtual void InputHandle()
        {
            if (!canUseSkills) return;
            if (characterIA != null) return;
            
            if (InputAxis != null)
            {
                if (InputAxis() != 0)
                {
                    UseSkill();
                }
            }
            if (InputCode != null)
            {
                if (InputCode())
                {
                    UseSkill();
                }
            }
        }

        #endregion

        public virtual void UseSkill()
        {
            if (!canUseSkills) return;
        }

        public virtual void PlaySound()
        {
            if (skillSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(skillSound);
            }
        }

        public virtual void IncrementPower(IncrementType i)
        {
            if (skillPower != null)
            {
                skillPower.Increment(i);
            }
        }
    }
}
