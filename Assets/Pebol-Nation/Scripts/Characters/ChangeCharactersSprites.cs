using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharactersSprites : MonoBehaviour
{
    [Header("Set Player")]
    public Goal.Team team;
    public RuntimeAnimatorController controler2;

    [Header("Set Spites")]
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer shoes;

    public void ChangeSkin(CharacteSkin skin)
    {
        if (team == Goal.Team.player2)
        {
            head.flipX = body.flipX = shoes.flipX = true;
            
            Vector3 tempPos = shoes.transform.position;
            tempPos.x = -0.137f;
            shoes.transform.position = tempPos;

            GetComponent<Animator>().runtimeAnimatorController = controler2;
        }

        head.sprite = skin.head;
        body.sprite = skin.body;
        shoes.sprite = skin.shoes;
    }
}
