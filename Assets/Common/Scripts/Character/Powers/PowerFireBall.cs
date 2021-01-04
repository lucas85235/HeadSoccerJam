using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse.Powers
{
    public class PowerFireBall : CharacterPower
    {
        [Header("Fire Ball Power")]
        public float activeTimer = 3f;
        public float callAttackTimer = 0.25f;
        public float kickForce = 600f;

        protected bool canActivePower = false;

        public override void UsePower()
        {
            canActivePower = true;

            Invoke("DisableActive", activeTimer);
        }

        protected virtual void DisableActive()
        {
            canActivePower = false;
        }

        protected virtual void PowerRoutine()
        {
            ball.activeKick = true;
            ball.BallRebound(Vector3.right, kickForce);

            DisableActive();
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Ball" && canActivePower)
            {
                ball.activeKick = false;
                ball.rb.velocity = Vector3.zero;

                CancelInvoke("DisableActive");
                Invoke("PowerRoutine", callAttackTimer);
            }
        }
    }
}
