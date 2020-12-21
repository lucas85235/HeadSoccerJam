using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LanguageManager : MonoBehaviour 
{    
    // colocar os arquivos de liguagem na pasta Resources
    // eles devem ser um json porem devem ter a extensão txt
    // devido a um bug que no android não e reconhecido caso
    // o arquivo tenha a extensão json

    [HideInInspector]
    public const string saveLanguageKey = "Language";
    public string SaveLanguageKey { get => saveLanguageKey; }
    private string languageSelected = "language_text_pt-br";

    public string[] LanguageOptions { get => languageOptions; }
    private string[] languageOptions = {
        "language_text_pt-br",
        "language_text_es-es",
        "language_text_en-us",
    };

    private Dictionary<string, string> localizedText;
    private string missingText = "Text or key not found!";
    private bool isReady = false;

    [Header("Use para mudar a linguagem em tempo de execução")]
    public Languages selectLanguage;
    private Languages currentLanguage;

    /// <summary> use para saber se já poder usar o sistema </summary>
    public bool IsReady() => isReady;

    /// <summary> salva a linguagem que for igual ao nome do arquivo </summary>
    public void SaveLanguage(string fileName) => PlayerPrefs.SetString(saveLanguageKey, fileName);

    /// <summary> carrega a linguagem que foi salve ou a padrão caso não tenha salvo </summary>
    public void LoadSaveLanguage() => LoadLocazidedText(languageSelected);

    /// <summary> Use esse evento para trocar os textos após a mudança de linguagem </summary>
    public Action OnChangeLanguage;

    public static LanguageManager Instance;

    void Awake() 
    {
        Instance = this;
        
        // Load Saved Key
        if (PlayerPrefs.HasKey(saveLanguageKey))
        {
            languageSelected = PlayerPrefs.GetString(saveLanguageKey);
            LoadLocazidedText(languageSelected);
        }
    }

    void Start()
    {
        currentLanguage = selectLanguage;
        OnChangeLanguage = () => {
            Debug.Log("Reload languages");
        };
    }

    void FixedUpdate()
    {
        #if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectLanguage = Languages.ptBr;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectLanguage = Languages.enUs;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectLanguage = Languages.esEs;

        #endif
        
        // Detecta a mudança de linguagem em tempo de execução
        if (selectLanguage != currentLanguage && isReady)
        {
            if (selectLanguage == Languages.ptBr)
                LoadLocazidedText(LanguageOptions[0]);
            else if (selectLanguage == Languages.esEs)
                LoadLocazidedText(LanguageOptions[1]);
            else if (selectLanguage == Languages.enUs)
                LoadLocazidedText(LanguageOptions[2]);

            currentLanguage = selectLanguage;
            if (OnChangeLanguage != null) OnChangeLanguage();
        }
    }

    // acha e carrega o arquivo com as traduções
    public void LoadLocazidedText(string fileName) 
    {
        isReady = false;

        localizedText = new Dictionary<string, string>();

        TextAsset file = Resources.Load(fileName) as TextAsset;
        var dataAsJson = file.text;
        LanguageData loaderData = JsonUtility.FromJson<LanguageData>(dataAsJson);
        
        for (int i = 0; i < loaderData.items.Length; i++)
        {
            localizedText.Add(loaderData.items[i].key, loaderData.items[i].value);
        }

        SaveLanguage(fileName);

        PlayerPrefs.SetString(saveLanguageKey, fileName);
        PlayerPrefs.Save();

        Debug.Log("Data loader, dictionary contains: " + localizedText.Count + " entries");
        isReady = true;
    }

    // retorna o valor definido para a chave
    public string GetKeyValue(string key) 
    {
        if (!isReady) LoadLocazidedText(LanguageOptions[0]);

        string resul = missingText;

        if (localizedText.ContainsKey(key)) 
        {
            resul = localizedText[key];
        }

        return resul;
    }
}

public enum Languages
{
    ptBr,
    esEs,
    enUs,
}
