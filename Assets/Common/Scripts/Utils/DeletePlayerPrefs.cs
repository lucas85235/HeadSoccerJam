using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse;

public class DeletePlayerPrefs : MonoBehaviour
{
    private void Update()
    {
        if (InputSystem.Instance.Interact())
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
