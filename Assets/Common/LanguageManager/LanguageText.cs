using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageText : MonoBehaviour
{
    private Text text;
    public string key;

    private IEnumerator Start()
    {
        text = GetComponent<Text>();    

        if (!LanguageManager.Instance.IsReady()) 
        {
            yield return null;
        }

        text.text = LanguageManager.Instance.GetKeyValue(key);
        LanguageManager.Instance.OnChangeLanguage += UpdateText;
    }

    private void UpdateText() 
    {
        text.text = LanguageManager.Instance.GetKeyValue(key);
    }

    void OnDestroy()
    {
        LanguageManager.Instance.OnChangeLanguage -= UpdateText;
    }
}
