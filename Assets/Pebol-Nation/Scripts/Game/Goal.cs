using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse;

public class Goal : MonoBehaviour
{
    public enum Team { player1, player2 }
    public Team team;


    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Ball" && !GameManager.Instance.GameOver)
        {
            if (team == Team.player1)
            {
                GameManager.Instance.UpdateScore(new Vector2(0, 1));
            }
            else GameManager.Instance.UpdateScore(new Vector2(1, 0));
        }
    }
}
