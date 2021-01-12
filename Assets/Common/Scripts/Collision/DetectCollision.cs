using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class DetectCollision : MonoBehaviour
    {
        [Header("Detect Settings")]
        public Transform owner;
        public float detectTimer = 0.2f;

        [Header("Layers")]
        public string[] detectingLayers;

        protected Collider detectCollider;
        protected bool detectCoolDown = true;

        public bool isDetected { get; protected set; }

        void Awake()
        {
            detectCollider = GetComponent<Collider>();
            isDetected = false;
        }

        protected virtual void OnTriggerStay(Collider other) 
        {
            foreach (var layer in detectingLayers)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer(layer) && detectCoolDown)
                {
                    if (other.transform != owner || owner == null)
                    {
                        detectCollider.enabled = false;
                        detectCoolDown = false;
                        isDetected = true;

                        Invoke("DetectCoolDown", detectTimer);                    
                    }
                }
            }
        }

        protected virtual void DetectCoolDown()
        {
            detectCollider.enabled = true;
            detectCoolDown = true;
            isDetected = false;
        }
    }
}
