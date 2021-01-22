using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class LoadSelectCharacter : MonoBehaviour
    {
        [SerializeField] 
        private Object[] models;

        private string saveModelsKey_ = "SELECTED-CHARACTER";
        public string SaveModelsKey { get => saveModelsKey_; }

        [Header("Start Settings")]
        public Transform defaultModel;

        public void SaveModel(string value) 
        { 
            Debug.Log("Load Save -> " + value);
            PlayerPrefs.SetString(saveModelsKey_, value);
            PlayerPrefs.Save();
        }

        /// <summary>
        ///  Carrega todos os modelos na pasta Resources/Player
        /// </summary>
        public void Load()
        {
            if (!PlayerPrefs.HasKey(saveModelsKey_))
            {
                SaveModel(defaultModel.name);
            }

            models = Resources.LoadAll("Player", typeof(GameObject));
        }

        /// <summary>
        ///  Retorna o GameObject que tiver o mesmo nome que foi salvo no PlayerPrefs
        /// </summary>
        public GameObject SelectedAllModels() 
        {
            if (models == null) models = Resources.LoadAll<GameObject>("Player");
            
            Debug.Log("Load -> " + PlayerPrefs.GetString(saveModelsKey_));
            var cName = PlayerPrefs.GetString(saveModelsKey_);

            for (int i = 0; i < models.Length; i++)
            {
                if (cName == models[i].name) 
                {
                    return models[i] as GameObject;
                }
            }

            Debug.LogError("LoadSelectCharacter not find model!");

            return Resources.Load<GameObject>(defaultModel.name);
        }

        public GameObject SelectedModel() 
        {
            Debug.Log("Load -> " + PlayerPrefs.GetString(saveModelsKey_));
            return Resources.Load<GameObject>("Player/" + PlayerPrefs.GetString(saveModelsKey_));
        }
    } 
}
