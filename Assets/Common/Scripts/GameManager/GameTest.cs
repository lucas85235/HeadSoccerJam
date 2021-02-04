using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class GameTest : MonoBehaviour
    {
        protected GameManager game;

        void Start()
        {
            game = FindObjectOfType<GameManager>();
        }

        void Update()
        {
            if (game.GameOver && InputSystem.Instance.Interact())
            {
                game.OnContinue();
            }
        }
    }
}
