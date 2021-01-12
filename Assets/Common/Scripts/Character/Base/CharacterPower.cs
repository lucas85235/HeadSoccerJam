using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse.Characters;

namespace MadeInHouse.Powers
{
    public class CharacterPower : MonoBehaviour
    {
        protected BallBehaviour ball;
        protected Character character;

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            ball = FindObjectOfType<BallBehaviour>();
            character = GetComponent<Character>();
        }

        public virtual void UsePower()
        {

        }
    }
}
