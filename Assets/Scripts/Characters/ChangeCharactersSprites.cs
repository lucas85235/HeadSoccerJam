using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharactersSprites : MonoBehaviour
{
    public Goal.Team team;

    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer shoes;

    public void ChangeSkin(CharacteSkin skin)
    {
        // if right mark x [x]

        head.sprite = skin.head;
        body.sprite = skin.body;
        shoes.sprite = skin.shoes;
    }
}
