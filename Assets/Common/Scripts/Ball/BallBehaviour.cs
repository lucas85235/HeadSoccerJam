using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class BallBehaviour : MonoBehaviour
    {
        public Rigidbody rb { get; set; }
        public bool activeKick = true;

        protected virtual void Start() 
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary> Add rebound to ball </summary>
        public virtual void BallRebound(Vector3 rebound, float kickForce)
        {
            if (activeKick)
            {
                rb.AddForce(rebound * kickForce, ForceMode.Force);
            }
        }
    }    
}
