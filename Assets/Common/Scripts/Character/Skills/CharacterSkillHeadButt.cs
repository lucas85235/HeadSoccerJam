using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillHeadButt : CharacterSkill
    {
        protected BallBehaviour ball;
        protected DetectGround detectGround;
        protected float axisInput;

        [Header("Stats")]
        [Range(140, 300)] public float buttForce = 180;

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

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (!canUseSkills) return;
            
            if (other.gameObject.tag == "Ball")
            {
                PlaySound();
                Vector3 rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(-0.5f, -1f), 0);

                // quando a bola só bate no jogador
                if (detectGround.IsGrounded() && axisInput == 0)
                {
                    rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 0);
                    // Debug.Log("Kick 1");
                }
                // quando a bola bate no jogador e ele esta pulando
                else if (!detectGround.IsGrounded() && axisInput == 0)
                {
                    rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(1f, 1.5f), 0);
                    // Debug.Log("Kick 2");
                }
                // quando a bola bate no jogador e ele não esta pulando mas se movendo
                else if (detectGround.IsGrounded() && axisInput != 0)
                {
                    rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(0.5f, 1f), 0);
                    // Debug.Log("Kick 3");
                }
                // quando a bola bate no jogador e ele esta pulando e se movendo
                else if (!detectGround.IsGrounded() && axisInput != 0)
                {
                    rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(0.5f, 1f), 0);
                    // Debug.Log("Kick 4");
                }

                if (characterIA != null)
                {
                    rebound.x = -rebound.x;
                }

                ball.BallRebound(rebound, buttForce);
                IncrementPower();
            }
        }
    }
}
