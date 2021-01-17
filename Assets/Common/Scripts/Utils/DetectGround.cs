using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    [RequireComponent(typeof(Collider))]

    public class DetectGround : MonoBehaviour
    {
        public LayerMask layerGround;

        [Header("Debug variables")]
        [SerializeField] protected bool isGround;
        [SerializeField] protected float distToGround;

        protected virtual void Start()
        {
            if (distToGround == 0)
            {
                distToGround = GetComponent<Collider>().bounds.extents.y;
            }
        }

        /// <summary> Detect contact with ground </summary>
        public virtual bool IsGrounded()
        {
            isGround = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f, layerGround);
            return isGround;
        }
    }
}