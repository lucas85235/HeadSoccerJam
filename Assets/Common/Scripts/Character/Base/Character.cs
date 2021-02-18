using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class Character : MonoBehaviour
    {
        protected CharacterSkill[] skills;
        public InputSystem input { get; protected set; }

        protected float stunnedTime = 3f;
        protected bool isStunned = false;

        protected bool isIA = false;

        [Header("Setup")]
        public Transform model;
        public CharacterType characterType = CharacterType.Player;
        public int playerIndex = 0;

        public enum CharacterType
        {
            Player,
            IA,
        }

        protected virtual void Awake()
        {
            skills = GetComponents<CharacterSkill>();

            if (GetComponent<CharacterIA>() != null)
            {
                isIA = true;
            }
        }

        public virtual void SetInputSystem(InputSystem inputSystem) 
        {
            if (characterType == CharacterType.Player)
            {
                input = inputSystem;
            }    
        }

        protected virtual void Update()
        {
            skills[0].IncrementPower(IncrementType.second);

            if (isIA) return;
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

