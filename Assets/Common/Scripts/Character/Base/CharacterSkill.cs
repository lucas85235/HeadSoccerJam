using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    [RequireComponent(typeof(Character))]

    public class CharacterSkill : MonoBehaviour
    {
        protected Animator anim;
        protected CharacterIA characterIA;
        protected CharacterSkillPower skillPower;
        
        [HideInInspector] public bool canUseSkills = true;

        [Header("Feedbakcs")]
        public AudioClip skillSound;

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            anim = GetComponent<Animator>();
            skillPower = GetComponent<CharacterSkillPower>();
            characterIA = GetComponent<CharacterIA>();
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

        protected virtual void IncrementPower()
        {
            if (skillPower != null)
            {
                skillPower.Increment();
            }
        }
    }
}
