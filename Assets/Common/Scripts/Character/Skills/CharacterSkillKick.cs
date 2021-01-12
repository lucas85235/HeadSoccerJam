using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillKick : CharacterSkill
    {
        protected BallBehaviour ball;
        protected DetectGround detectGround;
        protected float axisInput;

        [Header("Stats")]
        [Range(140, 300)] public float kickForce = 180;
        
        [Header("Settings")]
        public DetectCollision detectCollision;

        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Kick;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ball = FindObjectOfType<BallBehaviour>();
            detectGround = FindObjectOfType<DetectGround>();
        }

        public override void InputHandle()
        {
            base.InputHandle();
            axisInput = InputSystem.Instance.AxisX();
        }

        /// <summary> </summary>
        public override void UseSkill()
        {
            base.UseSkill();

            if (anim != null)
            {
                anim.SetTrigger("Kick");
            }
            
            if (detectCollision != null && !detectCollision.isDetected) 
            {
                return;
            }

            if (ball != null)
            {
                PlaySound();
                Vector3 rebound = new Vector3(Random.Range(3.0f, 3.5f), Random.Range(0, 0.5f));
                ball.rb.AddForce(rebound * kickForce, ForceMode.Force);
                IncrementPower();
            }
        }
    }
}
