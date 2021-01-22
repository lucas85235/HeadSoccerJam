using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace MadeInHouse
{
    [RequireComponent(typeof(LoadSelectCharacter))]

    public class SelectCharacter : MonoBehaviour
    {
        [SerializeField] 
        private List<Transform> models_;
        private LoadSelectCharacter load;

        [Header("Control variables")]
        public float modelsIndex = 0;

        [Header("Button setup")]
        public Button previous;
        public Button next;

        void Awake()
        {
            load = GetComponent<LoadSelectCharacter>();
            load.Load();

            if (load.defaultModel == null) 
            {
                Debug.LogError("In SelectCharacter the variable defaultModel is Mandatory");
                return;
            }
            
            models_ = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var model = transform.GetChild(i);
                models_.Add(model);

                if (model.name == PlayerPrefs.GetString(load.SaveModelsKey)) 
                {
                    model.gameObject.SetActive(true);
                    modelsIndex = i;
                }
                else model.gameObject.SetActive(false);
            }

            // Button setup

            if (previous != null)
            {
                previous.onClick.AddListener(() => {
                    modelsIndex--;
                    EnableModel();
                });                
            }

            if (next != null)
            {
                next.onClick.AddListener(() => {
                    modelsIndex++;
                    EnableModel();
                });                
            }

        }

        /// <summary>
        ///  Troca de modelo de acordo com o modelsIndex.
        ///  Se o indice for maior que o tamanho do array recebe zero,
        ///  se for menor que zero recebe o último valor do array.
        /// </summary>
        /// <param name="modelsIndex"> Seleciona o modelo que deve ser ativado </param>
        public void EnableModel()
        {
            if (modelsIndex > transform.childCount -1) modelsIndex = 0;
            else if (modelsIndex < 0) modelsIndex = transform.childCount - 1;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (i == modelsIndex) 
                {
                    models_[i].gameObject.SetActive(true);
                    load.SaveModel(models_[i].name);
                }
                else models_[i].gameObject.SetActive(false);
            }

        }

        /// <summary>
        ///  Retorna e Seta a lista de modelos
        /// </summary>
        public List<Transform> GetModels { get => models_; set => models_ = value; }
    }
}
