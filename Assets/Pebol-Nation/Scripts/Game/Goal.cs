using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MadeInHouse;
using MadeInHouse.Characters;

public class Goal : MonoBehaviour
{
    public enum Team { player1, player2 }
    public Team team;

    public CharacterSkill playerOne;
    public CharacterSkill playerTwo;

    private void Start() 
    {
        playerOne = GameObject.FindGameObjectWithTag("Player1").GetComponent<CharacterSkill>();    
        playerTwo = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterSkill>();    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Ball" && !GameManager.Instance.GameOver && GameManager.Instance.canScoring)
        {
            if (team == Team.player1)
            {
                GameManager.Instance.UpdateScore(new Vector2(0, 1));
                playerOne.IncrementPower(IncrementType.taking);
                playerTwo.IncrementPower(IncrementType.scoring);
            }
            else 
            {
                GameManager.Instance.UpdateScore(new Vector2(1, 0));
                playerOne.IncrementPower(IncrementType.scoring);
                playerTwo.IncrementPower(IncrementType.taking);
            }
        }
    }
}
