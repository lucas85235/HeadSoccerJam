using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "ScriptableObjects/Skins", order = 1)]
public class CharacteSkin : ScriptableObject
{
    public Sprite head;
    public Sprite body;
    public Sprite shoes;
}
