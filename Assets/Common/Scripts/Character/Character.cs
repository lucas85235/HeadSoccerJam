using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class Character : MonoBehaviour
    {
        protected CharacterSkill[] skills;
        
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
            if (skills == null) return;

            foreach (var skill in skills)
            {
                skill.InputHandle();
            }
        }
    }    
}

