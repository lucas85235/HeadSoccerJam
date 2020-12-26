using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LanguageData
{
    public LanguageItem[] items;
}

[System.Serializable]
public struct LanguageItem
{
    public string key;
    public string value;
}
