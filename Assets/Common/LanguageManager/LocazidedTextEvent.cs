using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocazidedTextEvent : MonoBehaviour
{
    public Languages language;

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
       {
           StartCoroutine(ChangeLanguage());
       });
    }

    private IEnumerator ChangeLanguage()
    {
        LanguageManager.Instance.selectLanguage = language;

        yield return null;

        if (language == Languages.ptBr)
            PlayerPrefs.SetString(LanguageManager.Instance.SaveLanguageKey, LanguageManager.Instance.LanguageOptions[0]);
        else if (language == Languages.esEs)
            PlayerPrefs.SetString(LanguageManager.Instance.SaveLanguageKey, LanguageManager.Instance.LanguageOptions[1]);
        else if (language == Languages.enUs)
            PlayerPrefs.SetString(LanguageManager.Instance.SaveLanguageKey, LanguageManager.Instance.LanguageOptions[2]);

        PlayerPrefs.Save();
    }
}
