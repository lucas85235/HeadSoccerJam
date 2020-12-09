﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    [RequireComponent(typeof(Character))]

    public class CharacterSkill : MonoBehaviour
    {
        [Header("Feedbakcs")]
        public AudioClip skillSound;

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {

        }

        #region Input Handle

        public Func<float> InputAxis;
        public Func<bool> InputCode;

        public virtual void InputHandle()
        {
            if (InputAxis != null)
            {
                if (InputAxis() != 0)
                {
                    Skill();
                }
            }
            if (InputCode != null)
            {
                if (InputCode())
                {
                    Skill();
                }
            }
        }

        #endregion

        public virtual void Skill()
        {

        }

        public virtual void PlaySound()
        {
            if (skillSound != null)
            {
                AudioSource.PlayClipAtPoint(skillSound, Camera.main.transform.position);
            }
        }
    }
}