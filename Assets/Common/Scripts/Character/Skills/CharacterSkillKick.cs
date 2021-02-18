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
        public bool automaticKick = false;

        [Header("Settings")]
        public DetectCollision ballCollision;
        public DetectCollision playerCollision;

        protected override IEnumerator Start()
        {
            base.Start();

            yield return new WaitUntil( () => character.input != null );

            InputCode = character.input.Kick;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ball = FindObjectOfType<BallBehaviour>();
            detectGround = FindObjectOfType<DetectGround>();

            ballCollision.OnDetect += AutomaticKickBall;
            playerCollision.OnDetect += AutomaticKickPlayer;
        }

        public override void InputHandle()
        {
            base.InputHandle();

            if (character.input != null)
            {
                axisInput = character.input.AxisX();
            }
        }

        protected virtual void AutomaticKickBall()
        {
            float angel = Vector3.Angle(transform.forward, ball.transform.position - transform.position);

            if (automaticKick && Mathf.Abs(angel) > 90 && canKick)
            {
                UseSkill();
                Debug.Log("Kick Ball");
            }
        }

        protected virtual void AutomaticKickPlayer()
        {
            var angle = Vector3.Angle(transform.forward, ball.transform.position - transform.position);

            if (automaticKick && Mathf.Abs(angle) > 30 && canKick)
            {
                KickOnPlayer();
                Debug.Log("Kick Player");
            }
        }

        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, ball.transform.position - transform.position);
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

            Debug.Log("Use Kick");

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

                    if (characterIA != null)
                    {
                        rebound.x = -rebound.x;
                    }

                    ball.rb.velocity = Vector2.zero;
                    ball.rb.AddForce(rebound * kickForce, ForceMode.Force);
                    IncrementPower(IncrementType.interact);
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
                    life.DecreaseLife();
                }
            }
        }

        protected virtual void KickCoolDown()
        {
            canKick = true;
        }

        private void OnDestroy()
        {
            ballCollision.OnDetect = null;
        }
    }
}
