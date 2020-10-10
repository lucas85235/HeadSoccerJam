using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameController game;

    public enum Team { left, right }
    public Team team;

    private void Start() 
    {
        game = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Ball" && !game.matchIsEnded)
        {
            if (team == Team.left)
            {
                game.UpdateScore(new Vector2(0, 1));
            }
            else game.UpdateScore(new Vector2(1, 0));
        }
    }
}
