using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum Team { player1, player2 }
    public Team team;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Ball" && !GameController.get.matchIsEnded && !GameController.get.waitScore)
        {
            if (team == Team.player1)
            {
                GameController.get.UpdateScore(new Vector2(0, 1));
            }
            else GameController.get.UpdateScore(new Vector2(1, 0));
        }
    }
}
