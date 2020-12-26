using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    public string loaderScene;
    public bool loadSaveLanguage = true;

    private IEnumerator Start()
    {
        if (loadSaveLanguage) 
        {
            if (PlayerPrefs.HasKey(LanguageManager.Instance.SaveLanguageKey)) 
            {
                LanguageManager.Instance.LoadSaveLanguage();
            }        
        }

        while(!LanguageManager.Instance.IsReady()) 
        {
            yield return null;        
        }

        SceneManager.LoadScene(loaderScene);
    }
}
