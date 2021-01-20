using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInHouse.Characters
{
    public class CharacterSkillLife : CharacterSkill
    {
        protected Rigidbody rb;
        protected Collider thisCollider;
        protected DetectGround detectGround;

        [Header("Life Setup")]
        public Slider lifeSlider;
        public int maxLife = 5;

        [Header("Knockout Setup")]
        public float knockoutTimer = 1.5f;
        public float invencibleTimer = 0.5f;
        public float reviveAnimCompensationTimer = 0.5f;

        public bool isKnockout { get; protected set; }
        public bool extendKnockTimer { get; protected set; }
        
        protected override void Initialize()
        {
            base.Initialize();

            rb = GetComponent<Rigidbody>();
            thisCollider = GetComponent<Collider>();
            detectGround = GetComponent<DetectGround>();

            var canvas = GameObject.Find("PlayerHud").transform;
            lifeSlider = Instantiate(lifeSlider, canvas);
            lifeSlider.maxValue = maxLife;
            lifeSlider.value = maxLife;

            isKnockout = false;
            extendKnockTimer = false;
        }

        public virtual void SetMaxLife()
        {
            lifeSlider.value = maxLife;

            if (isKnockout)
            {
                extendKnockTimer = false;
                
                CancelInvoke();

                Invoke("ReviveAnimation", 0f);
                Invoke("Revive", reviveAnimCompensationTimer);
            }
        }

        public virtual void DecreaseLife(int decrease = 1)
        {
            base.UseSkill();
            if (isKnockout == true) return;

            lifeSlider.value -= decrease;

            if (lifeSlider.value < 1)
            {
                Knockout();
            }
        }

        public virtual void ExtendKnockTimer()
        {
            extendKnockTimer = true;
        }

        protected virtual void Knockout()
        {
            StartCoroutine( DisableCollider() );

            if (anim != null)
            {
                anim.SetTrigger("Fallen");
            }

            isKnockout = true;
            character.SetCanUseSkills(false);
            
            Invoke("ReviveAnimation", knockoutTimer - reviveAnimCompensationTimer);
            Invoke("Revive", knockoutTimer);
        }

        protected virtual void ReviveAnimation()
        {
            if ( ExtendKnockBehaviour() )
            {
                extendKnockTimer = false;
                return;
            }

            if (anim != null)
            {
                anim.SetTrigger("FallenExit");
            }
        }

        /// <summary> Can move and use other skills </summary>
        protected virtual void Revive()
        {
            extendKnockTimer = false;
            lifeSlider.value = maxLife;
            character.SetCanUseSkills(true);

            Invoke("ResetKnockout", invencibleTimer);
        }

        /// <summary> Can receive damage </summary>
        protected virtual void ResetKnockout()
        {
            isKnockout = false;
        }

        protected virtual bool ExtendKnockBehaviour()
        {
            if (extendKnockTimer)
            {
                Debug.Log("Extend");

                CancelInvoke();

                Invoke("ReviveAnimation", knockoutTimer - reviveAnimCompensationTimer);
                Invoke("Revive", knockoutTimer);
            }

            return extendKnockTimer;
        }

        protected virtual IEnumerator DisableCollider()
        {
            yield return new WaitUntil( () => detectGround.IsGrounded());

            if (!isKnockout) yield break;

            rb.isKinematic = true;
            rb.useGravity = false;
            thisCollider.enabled = false;
        
            yield return new WaitUntil( () => lifeSlider.value == maxLife);

            rb.isKinematic = false;
            rb.useGravity = true;
            thisCollider.enabled = true;
        }
    }
}
