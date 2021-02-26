using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MadeInHouse.Characters;

namespace MadeInHouse
{
    public class GameTournament : GameManager
    {
        [Header("Tournament")]
        public Text levelText;
        public CupHud cupHud;
        public LevelSetup[] levels;

        [Header("Debug")]
        public int currentLevel = 0;

        protected override void Start()
        {
            NextLevelSetup();

            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            if (GameOver && currentLevel == levels.Length && !cupHud.win.activeSelf)
            {
                Debug.Log("End Tournement");

                if (matchScore.x > matchScore.y)
                {
                    cupHud.win.SetActive(true);
                }
                else cupHud.lose.SetActive(true);

                return;
            }

            if (GameOver && InputSystem.Instance.Interact())
            {
                NextLevelAction();
            }
        }

        // in case of necessary set in button event
        public virtual void NextLevelAction()
        {
            NextLevelSetup();
            OnContinue();
        }

        protected virtual void NextLevelSetup()
        {
            cupHud.next.SetActive(false);

            var findPlayerTwo = GameObject.FindWithTag("Player2");
            if (findPlayerTwo != null)
            {
                Destroy(findPlayerTwo);
            }

            playerTwo = levels[currentLevel].adversary.transform;
            playerTwo = Instantiate(playerTwo, new Vector3(7, -0.5f, -2), playerTwo.rotation);
            playerTwo.tag = "Player2";

            playerTwo.GetComponent<CharacterIA>().SetCpuLevel(levels[currentLevel].difficulty);
        
            var characterTwo = playerTwo.GetComponent<Character>();
            characterTwo.SetCanUseSkills(false);
            characterTwo.playerIndex = 1;

            currentLevel++;
            levelText.text = currentLevel.ToString();

            Debug.Log("Current Level " + currentLevel);
        }

        protected override void WhoWon()
        {
            int coinEarm = 0;

            // Empate
            if (matchScore.x == matchScore.y)
            {
                cupHud.lose.SetActive(true);
            }
            // left player won
            else if (matchScore.x < matchScore.y)
            {
                cupHud.lose.SetActive(true);
            }
            // right player won
            else if (matchScore.x > matchScore.y)
            {
                cupHud.next.SetActive(true);
                coinEarm = Random.Range(20, 50) * currentLevel;
            }

            LegendCoin.Instance.EarnLegendCoin(coinEarm);
            hud.coinEarnedText.text = "+" + coinEarm;
            hud.coinEarnedText.gameObject.SetActive(true);
        }

        [System.Serializable]
        public struct LevelSetup
        {
            public CharacterIA.DifficultyLevels difficulty;
            public CharacterIA adversary;
        }


        [System.Serializable]
        public struct CupHud
        {
            public GameObject lose;
            public GameObject next;
            public GameObject win;
        }
    }
}
