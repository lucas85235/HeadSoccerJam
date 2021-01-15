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

        [Header("Layer")]
        public string detectingLayer;

        protected Collider detectCollider;
        protected bool detectCoolDown = true;

        public Transform otherDetected { get; protected set; }
        public bool isDetected { get; protected set; }

        void Awake()
        {
            detectCollider = GetComponent<Collider>();
            isDetected = false;
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(detectingLayer) && detectCoolDown)
            {
                if (other.transform != owner || owner == null)
                {
                    otherDetected = other.transform;
                    detectCollider.enabled = false;
                    detectCoolDown = false;
                    isDetected = true;

                    Invoke("DetectCoolDown", detectTimer);
                }
            }
        }

        protected virtual void DetectCoolDown()
        {
            otherDetected = null;
            detectCollider.enabled = true;
            detectCoolDown = true;
            isDetected = false;
        }
    }
}
