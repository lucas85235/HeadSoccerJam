using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class BallBehaviour : MonoBehaviour
    {
        public Rigidbody rb { get; set; }

        protected virtual void Start() 
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary> Add rebound to ball </summary>
        public virtual void BallRebound(Vector3 rebound, float kickForce)
        {
            rb.AddForce(rebound * kickForce, ForceMode.Force);
        }
    }    
}
