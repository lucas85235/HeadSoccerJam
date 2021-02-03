using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MadeInHouse.Characters;

namespace MadeInHouse
{
    public class GameTournament : MonoBehaviour
    {
        protected GameManager game;
        
        [Header("Tournament Settings")]
        public Text levelText;
        public LevelSetup[] levels;

        [Header("Debug")]
        public int currentLevel = 0;

        void Start()
        {
            game = FindObjectOfType<GameManager>();
            NextLevelSetup();
        }

        void Update()
        {
            if (game.GameOver && currentLevel == levels.Length)
            {
                Debug.Log("End Tournement");
                return;
            }
            
            if (game.GameOver && InputSystem.Instance.Interact())
            {
                NextLevelSetup();
                game.OnContinue();
            }
        }

        protected virtual void NextLevelSetup()
        {
            var playerTwo = GameObject.FindWithTag("Player2");
            if (playerTwo != null)
            {
                Destroy(playerTwo);
            }

            game.playerTwo = levels[currentLevel].adversary.transform;
            game.playerTwo = Instantiate(game.playerTwo, new Vector3(7, -0.5f, -2), game.playerTwo.rotation);

            game.playerTwo.GetComponent<CharacterIA>().SetCpuLevel(levels[currentLevel].difficulty);
            game.playerTwo.GetComponent<Character>().SetCanUseSkills(false);

            currentLevel++;
            levelText.text = currentLevel.ToString();

            Debug.Log("Current Level " + currentLevel);
        }

        [System.Serializable]
        public struct LevelSetup
        {
            public CharacterIA.DifficultyLevels difficulty;
            public CharacterIA adversary;
        }
    }
}
