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
        protected Character otherCharacter;

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            ball = FindObjectOfType<BallBehaviour>();
            character = GetComponent<Character>();
            otherCharacter = GameObject.FindGameObjectWithTag("Player2").GetComponent<Character>();
        }

        public virtual void UsePower()
        {

        }
    }
}
