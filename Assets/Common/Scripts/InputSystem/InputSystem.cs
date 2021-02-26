using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MadeInHouse.Characters;

namespace MadeInHouse
{
    public class InputSystem : MonoBehaviour
    {
        private InputHandle inputHandle;

        public int index = -1;

        public static InputSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private IEnumerator Start() 
        {
            inputHandle = GetComponent<InputHandle>();
            index = GetComponent<PlayerInput>().playerIndex;

            if (GameManager.Instance != null)
            {
                yield return new WaitUntil( () => GameManager.Instance.isReady);

                foreach (var character in FindObjectsOfType<Character>())
                {
                    if (character.playerIndex == index) character.SetInputSystem(this);
                }                
            }
        }

        public virtual float AxisX() { return inputHandle.axisX; }
        public virtual bool Jump() { return inputHandle.jump; }
        public virtual bool Kick() { return inputHandle.kick; }
        public virtual bool Power() { return inputHandle.power; }

        public virtual bool Interact() 
        { 
            if (inputHandle == null)
            {
                return false;
            }
            return inputHandle.interact; 
        }

        public virtual bool Escape() 
        { 
            if (inputHandle == null)
            {
                return false;
            }
            return inputHandle.escape; 
        }
    }
}

