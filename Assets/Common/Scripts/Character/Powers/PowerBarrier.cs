using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse.Characters;

namespace MadeInHouse.Powers
{
    public class PowerBarrier : CharacterPower
    {
        [Header("Setup")]
        public GameObject barrier;
        public float powerTimer = 3f;

        [Header("Spaw Position")]
        public SpawType spawType = SpawType.DynamicPosition;
        [Range(-3f, 3f)] public float spawDynamicPosition = 2f;
        public Vector3 spawFixedPosition = new Vector3(-5, .85f, -2);

        protected override void Initialize()
        {
            base.Initialize();

            var initialPos = barrier.transform.position;

            if (spawType == SpawType.FixedPostion)
            {
                initialPos = spawFixedPosition;
            }
            
            barrier = Instantiate(barrier, initialPos, barrier.transform.rotation);
            barrier.SetActive(false);
        }

        public override void UsePower()
        {
            if (spawType == SpawType.DynamicPosition)
            {
                var spawPos = barrier.transform.position;
                spawPos.x = transform.position.x + spawDynamicPosition;
                barrier.transform.position = spawPos;
            }

            barrier.SetActive(true);
            Invoke("DisablePower", powerTimer);
        }

        protected virtual void DisablePower()
        {
            barrier.SetActive(false);
        }
        
        public enum SpawType
        {
            DynamicPosition,
            FixedPostion
        }
    }
}
