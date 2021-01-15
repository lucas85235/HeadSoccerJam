using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class Character : MonoBehaviour
    {
        protected CharacterSkill[] skills;

        protected float stunnedTime = 3f;
        protected bool isStunned = false;

        void Start()
        {
            skills = GetComponents<CharacterSkill>();
        }

        void Update()
        {
            SkillsHandle();
        }

        protected virtual void SkillsHandle() 
        {
            if (skills == null || isStunned) return;

            foreach (var skill in skills)
            {
                skill.InputHandle();
            }
        }

        public virtual void SetCanUseSkills(bool state)
        {
            foreach (var skill in skills)
            {
                skill.canUseSkills = state;
            }
        }
    }    
}

