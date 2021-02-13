using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class DragRotation : MonoBehaviour
    {
        [Header("Drag Setup")]
        public GameObject characterScreen;
        public float rotationSpeed = 400f;

        [Header("On Enable")]
        public bool resetRotation = false;
        public Quaternion defaultRotation;

        protected Rigidbody rb_;
        protected bool isDragging;

        protected bool canDragging 
        { 
            get 
            { 
                if (characterScreen != null)
                {
                    return characterScreen.activeSelf; 
                }
                else return true;
            }
        }

        protected virtual void Start()
        {
            rb_ = GetComponent<Rigidbody>();
            isDragging = false;
        }

        protected virtual void Update()
        {
            if (canDragging)
            {
                if (Input.GetMouseButton(0))
                {
                    isDragging = true;
                }
                else if (Input.GetMouseButtonUp(0)) isDragging = false;                
            }
        }

        protected virtual void FixedUpdate()
        {
            if (isDragging && canDragging)
            {
                float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
                rb_.AddTorque(Vector3.down * x);
            }
        }

        protected virtual void OnEnable()
        {
            if (resetRotation)
            {
                if (rb_ == null) rb_ = GetComponent<Rigidbody>();
                rb_.angularVelocity = Vector3.zero;
                transform.localRotation = defaultRotation;
                transform.localPosition = Vector3.zero;
            }
        }

        protected virtual void OnDisable()
        {
            isDragging = false;
        }
    }
}
