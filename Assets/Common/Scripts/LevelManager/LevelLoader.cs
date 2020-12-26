using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadeInHouse
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadLevel(string level)
        {
            SceneManager.LoadScene(level);
        }
    }
}
