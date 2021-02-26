using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class SetControlScheme : MonoBehaviour
{

    [Header("Setup")]
    public ControlType controlType;
    public GameObject playerPrefab;

    public enum ControlType
    {
        Keyboard,
        Gamepad,
    }

    void Start()
    {
        if (controlType == ControlType.Keyboard)
        {
            PlayerInput.Instantiate(playerPrefab, pairWithDevice: Keyboard.current);
        }
        else if (controlType == ControlType.Gamepad)
        {
            PlayerInput.Instantiate(playerPrefab, pairWithDevice: Gamepad.current);
        }
    }
}
