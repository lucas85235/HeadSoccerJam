using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class InputSystem : MonoBehaviour
    {
        [Header("Input Settings")]
        public string axisX = "Horizontal";
        public string axisY = "Vertical";
        public KeyCode jumpInput = KeyCode.Z;
        public KeyCode kickInput = KeyCode.X;
        public KeyCode powerInput = KeyCode.E;
        public KeyCode interactInput = KeyCode.F;
        public KeyCode escapeInput = KeyCode.Escape;

        public static InputSystem Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        public virtual float AxisX() { return Input.GetAxis("Horizontal"); }
        public virtual float AxisY() { return Input.GetAxis("Vertical"); }
        public virtual bool Jump() { return Input.GetKeyDown(jumpInput); }
        public virtual bool Kick() { return Input.GetKeyDown(kickInput); }
        public virtual bool Power() { return Input.GetKeyDown(powerInput); }
        public virtual bool Interact() { return Input.GetKeyDown(interactInput); }
        public virtual bool Escape() { return Input.GetKeyDown(escapeInput); }
    }
}

