using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Characters
{
    public class CharacterSkillKick : CharacterSkill
    {
        protected Animator anim;
        protected BallBehaviour ball;
        protected CharacterSkillJump jumpSkill;
        protected float axisInput;
        protected bool canKick;

        [Header("Stats")]
        [Range(140, 240)] public float kickForce = 180;
        
        protected override void Start()
        {
            base.Start();
            InputCode = InputSystem.Instance.Kick;
        }

        protected override void Initialize()
        {
            base.Initialize();
            anim = GetComponent<Animator>();
            jumpSkill = GetComponent<CharacterSkillJump>();
            ball = FindObjectOfType<BallBehaviour>();
        }

        public override void InputHandle()
        {
            base.InputHandle();
            axisInput = InputSystem.Instance.AxisX();
        }

        /// <summary> </summary>
        public override void Skill()
        {
            base.Skill();
            anim.SetTrigger("Kick");

            if (canKick && ball != null)
            {
                PlaySound();
                Vector3 rebound = new Vector3(Random.Range(1.5f, 2f), Random.Range(-0.5f, -1f), 0);
                ball.rb.AddForce(rebound * kickForce, ForceMode.Force);
                IncrementPower();
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ball")
            {
                canKick = true;
                Vector3 rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(-0.5f, -1f), 0);

                // quando a bola só bate no jogador
                if (jumpSkill.IsGrounded() && axisInput == 0)
                {
                    rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), 0);
                    Debug.Log("Kick 1");
                }
                // quando a bola bate no jogador e ele esta pulando
                else if (!jumpSkill.IsGrounded() && axisInput == 0)
                {
                    rebound = new Vector3(Random.Range(0.5f, 1f), Random.Range(1f, 1.5f), 0);
                    Debug.Log("Kick 2");
                }
                // quando a bola bate no jogador e ele não esta pulando mas se movendo
                else if (jumpSkill.IsGrounded() && axisInput != 0)
                {
                    rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(0.5f, 1f), 0);
                    Debug.Log("Kick 3");
                }
                // quando a bola bate no jogador e ele esta pulando e se movendo
                else if (!jumpSkill.IsGrounded() && axisInput != 0)
                {
                    rebound = new Vector3(Random.Range(1f, 1.5f), Random.Range(0.5f, 1f), 0);
                    Debug.Log("Kick 4");
                }

                ball.BallRebound(rebound, kickForce);
                IncrementPower();
            }
        }

        protected virtual void OnCollisionExit(Collision other)
        {
            if (other.gameObject.tag == "Ball")
            {
                StartCoroutine(KickTimer());
            }
        }
        
        /// <summary> Tolerance to kick after collision exit  </summary>
        protected virtual IEnumerator KickTimer()
        {
            yield return new WaitForSeconds(0.15f);
            canKick = false;
        }
    }
}
