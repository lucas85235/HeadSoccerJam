using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInHouse
{
    public class LegendCoin : MonoBehaviour
    {
        public Text coinText;
        public int currentLegendCoinAmount;
        public bool isReady = false;

        protected const string coinSaveKey = "LegendCoin";

        public static LegendCoin Instance;

        protected virtual void Awake()
        {
            Instance = this;
            LoadLegendCoins();
            UpdateCoinText();
        }

        #if UNITY_EDITOR
        protected virtual void Update()
        {
            if (InputSystem.Instance != null && InputSystem.Instance.Interact())
            {
                EarnLegendCoin(10);
            }
        }
        #endif

        public virtual void LoadLegendCoins()
        {
            if (!PlayerPrefs.HasKey(coinSaveKey))
            {
                Debug.Log("Saved key for the first time");
                PlayerPrefs.SetInt(coinSaveKey, 0);
                PlayerPrefs.Save();
            }

            currentLegendCoinAmount = PlayerPrefs.GetInt(coinSaveKey);
            isReady = true;
        }

        public virtual void EarnLegendCoin(int value)
        {
            if (!isReady) LoadLegendCoins();

            currentLegendCoinAmount += value;
            PlayerPrefs.SetInt(coinSaveKey, currentLegendCoinAmount);
            PlayerPrefs.Save();

            UpdateCoinText();
        }

        public virtual void UpdateCoinText()
        {
            if (coinText != null)
            {
                coinText.text = currentLegendCoinAmount.ToString();
            }
        }
    }
}
