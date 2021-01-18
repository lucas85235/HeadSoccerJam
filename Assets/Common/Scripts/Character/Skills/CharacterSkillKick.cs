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
        protected bool canKick = true;

        [Header("Stats")]
        [Range(140, 300)] public float kickForce = 180;
        public float kickCoolDown = 0.3f;

        [Header("Settings")]
        public DetectCollision ballCollision;
        public DetectCollision playerCollision;

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

            if (!canUseSkills) return;
            
            if (canKick)
            {
                canKick = false;
                Invoke("KickCoolDown", kickCoolDown);
            }
            else return;
            
            if (anim != null)
            {
                anim.SetTrigger("Kick");
            }

            if (playerCollision != null && playerCollision.isDetected)
            {
                KickOnPlayer();
            }

            if (ballCollision != null && ballCollision.isDetected)
            {
                if (ball != null)
                {
                    PlaySound();
                    Vector3 rebound = new Vector3(Random.Range(3.0f, 3.5f), Random.Range(0, 0.5f));
                    ball.rb.velocity = Vector2.zero;
                    ball.rb.AddForce(rebound * kickForce, ForceMode.Force);
                    IncrementPower();
                }
            }
        }

        protected virtual void KickOnPlayer()
        {
            if (playerCollision.otherDetected != null)
            {
                var life = playerCollision.otherDetected.GetComponent<CharacterSkillLife>();

                if (life != null)
                {
                    life.UseSkill();
                }
            }
        }

        protected virtual void KickCoolDown()
        {
            canKick = true;
        }
    }
}
