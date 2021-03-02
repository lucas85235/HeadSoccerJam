using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace MadeInHouse
{
    public class InputHandle : MonoBehaviour
    {
        [Header("Debug")]
        public float axisX = 0;
        public bool jump;
        public bool kick;
        public bool power;
        public bool interact;
        public bool escape;

        public virtual void OnMove(CallbackContext context)
        {
            axisX = context.ReadValue<float>();
        }

        public virtual void OnJump(CallbackContext context)
        {
            jump = context.ReadValueAsButton();
        }

        public virtual void OnKick(CallbackContext context)
        {
            kick = context.ReadValueAsButton();
        }

        public virtual void OnPower(CallbackContext context)
        {
            power = context.ReadValueAsButton();
        }

        public virtual void OnInteract(CallbackContext context)
        {
            interact = context.ReadValueAsButton();
        }

        public virtual void OnEscape(CallbackContext context)
        {
            escape = context.ReadValueAsButton();
        }
    }
}
