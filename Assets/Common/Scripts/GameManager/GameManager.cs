using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MadeInHouse.Characters;

using Random = UnityEngine.Random;

namespace MadeInHouse
{
    public class GameManager : MonoBehaviour
    {
        protected LoadSelectCharacter load;
        public bool canScoring { get; private set; }

        [Header("Instantiete")]
        public Transform playerOne;
        public Transform playerTwo;
        public BallBehaviour ball;

        [Header("Match")]
        public float timeToStart = 3f;
        public float matchTime = 60f;
        public float restartMatchTimer = 1f;
        public float endMatchTimer = 1.25f;
        public float continueTimer = 2.5f;
        public bool autoContinueAfterEnd = true;

        [Header("FeedBacks")]
        public Animator goalAnimtion;

        [Header("Hud")]
        public SoccerHud hud;

        [Header("Sound")]
        public MatchSounds matchSounds;

        protected Vector2 matchScore;
        protected bool gameOver;
        public bool GameOver { get => gameOver; }

        public static GameManager Instance;
        protected virtual void Awake()
        {
            Instance = this;
            Time.timeScale = 1f;

            load = new LoadSelectCharacter();
            load.Load();

            ball = Instantiate(ball, new Vector3(0, -0.5f, -2), Quaternion.identity);
        }

        protected virtual void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            gameOver = false;

            if (playerOne == null)
            {
                playerOne = load.SelectedAllModels().transform;
            }

            playerOne = Instantiate(playerOne, new Vector3(-7, -0.5f, -2), playerOne.rotation);
            playerOne.GetComponent<Character>().SetCanUseSkills(false);

            if (playerTwo != null && !playerTwo.gameObject.scene.IsValid())
            {
                playerTwo = Instantiate(playerTwo, new Vector3(7, -0.5f, -2), playerTwo.rotation);
                var playerTwoIA = playerTwo.GetComponent<CharacterIA>();
                playerTwoIA.SetCpuLevel(playerTwoIA.cpuLevel);
                playerTwo.GetComponent<Character>().SetCanUseSkills(false);
            }

            Invoke("OnMatchStart", timeToStart);
        }

        protected virtual void Update()
        {
            if (InputSystem.Instance.Escape())
            {
                LevelLoader.Instance.LoadLevel("MainMenu");
            }
        }

        #region Match Events
        protected virtual void OnMatchStart()
        {
            Debug.Log("Start");

            playerOne.GetComponent<Character>().SetCanUseSkills(true);
            playerTwo.GetComponent<Character>().SetCanUseSkills(true);

            playerOne.GetComponent<CharacterSkillLife>().SetMaxLife();
            playerTwo.GetComponent<CharacterSkillLife>().SetMaxLife();
            
            playerOne.GetComponent<CharacterSkillPower>().powerSlider.value = 0;
            playerTwo.GetComponent<CharacterSkillPower>().powerSlider.value = 0;

            gameOver = false;
            canScoring = true;

            matchScore = new Vector2(0, 0);
            UpdateScoreText();

            hud.endMatchText.gameObject.SetActive(false);
            hud.coinEarnedText.gameObject.SetActive(false);
            SoundManager.Instance.PlayClipAtPoint(matchSounds.startWhistle);
            ResetPositions();

            StartCoroutine(MatchTimerRoutine());
        }
        
        protected virtual void OnAfterScoring()
        {
            ResetPositions();
            StopBallMovemnt();
            canScoring = true;
            SoundManager.Instance.PlayClipAtPoint(matchSounds.restartWhistle);
            Invoke("StartBallMovemnt", restartMatchTimer);
        }

        protected virtual void OnMatchEnd()
        {
            gameOver = true;
            WhoWon();

            if (autoContinueAfterEnd)
            {
                Invoke("OnContinue", continueTimer);
            }
        }

        public virtual void OnContinue()
        {
            gameOver = false;

            hud.endMatchText.gameObject.SetActive(false);
            hud.coinEarnedText.gameObject.SetActive(false);

            playerOne.position = new Vector3(-7, -0.5f, -2);
            playerTwo.position = new Vector3(7, -0.5f, -2);
            ball.transform.position = new Vector3(0, -0.5f, -2);

            playerOne.GetComponent<Character>().SetCanUseSkills(false);
            playerTwo.GetComponent<Character>().SetCanUseSkills(false);

            Invoke("OnMatchStart", timeToStart);
        }
        #endregion

        protected virtual void WhoWon()
        {
            int coinEarm = 0;

            // Empate
            if (matchScore.x == matchScore.y)
            {
                hud.endMatchText.text = "<color=#E15253>EMPATE</color>";
                coinEarm = Random.Range(10, 19);
            }
            // left player won
            else if (matchScore.x < matchScore.y)
            {
                hud.endMatchText.text = "<color=#E15253>DERROTA</color>";
                coinEarm = Random.Range(1, 9);
            }
            // right player won
            else if (matchScore.x > matchScore.y)
            {
                hud.endMatchText.text = "VITÓRIA";
                coinEarm = Random.Range(30, 50);
            }

            LegendCoin.Instance.EarnLegendCoin(coinEarm);
            hud.coinEarnedText.text = "+" + coinEarm;
            hud.coinEarnedText.gameObject.SetActive(true);            
        }

        protected virtual void ResetPositions()
        {
            playerOne.position = new Vector3(-7, -0.5f, -2);
            playerTwo.position = new Vector3(7, -0.5f, -2);
            ball.transform.position = new Vector3(0, 3.75f, -2);
        }

        protected virtual void StopBallMovemnt()
        {
            ball.rb.isKinematic = true;
            ball.rb.useGravity = false;
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;
        }

        protected virtual void StartBallMovemnt()
        {
            ball.rb.isKinematic = false;
            ball.rb.useGravity = true;
        }

        public virtual void UpdateScore(Vector2 score)
        {
            // Control Variable
            canScoring = false;

            // Update Score
            matchScore += score;
            UpdateScoreText();

            // Feedback
            SoundManager.Instance.PlayClipAtPoint(matchSounds.afterScore);
            StartCoroutine(WhenScoring());
        }

        protected virtual void UpdateScoreText()
        {
            hud.leftScoreText.text = matchScore.x.ToString();
            hud.rightScoreText.text = matchScore.y.ToString();            
        }

        protected virtual IEnumerator WhenScoring()
        {
            Time.timeScale = 0.5f;

            if (goalAnimtion != null)
                goalAnimtion.SetTrigger("Goal");

            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 1f;

            OnAfterScoring();
        }

        /// <summary> This caroutine handle with match count time and match end </summary>
        protected virtual IEnumerator MatchTimerRoutine()
        {
            // Time handle
            float timer = matchTime;
            while (timer > 0)
            {
                hud.timerText.text = timer.ToString();
                yield return new WaitForSeconds(1);
                timer -= 1;
            }

            // End handle
            SoundManager.Instance.PlayClipAtPoint(matchSounds.endWhistle);
            
            hud.timerText.text = timer.ToString();
            hud.endMatchText.text = "Fim da Partida";
            hud.endMatchText.gameObject.SetActive(true);

            if (!canScoring) yield return new WaitUntil( () => canScoring);

            // Controls Variables
            canScoring = false;
            gameOver = false;

            Invoke("OnMatchEnd", endMatchTimer);
        }        
    }

    [System.Serializable]
    public struct SoccerHud
    {
        public Text timerText;
        public Text leftScoreText;
        public Text rightScoreText;
        public Text endMatchText;
        public Text coinEarnedText;
    }

    [System.Serializable]
    public struct MatchSounds
    {
        public AudioClip startWhistle;
        public AudioClip restartWhistle;
        public AudioClip endWhistle;
        public AudioClip afterScore;
    }
}
