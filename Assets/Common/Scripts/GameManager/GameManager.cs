using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MadeInHouse
{
    public class GameManager : MonoBehaviour
    {
        public bool canScoring { get; private set; }

        [Header("Instantiete")]
        public Transform playerOne;
        public Transform playerTwo;
        public BallBehaviour ball;

        [Header("Match")]
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
        }

        protected virtual void Start()
        {
            playerOne = Instantiate(playerOne, new Vector3(6, -0.2f, -2), Quaternion.identity);
            playerTwo = Instantiate(playerTwo, new Vector3(-6, -0.2f, -2), Quaternion.identity);
            ball = Instantiate(ball, new Vector3(0, 3.75f, -2), Quaternion.identity);
            ResetPositions();

            Initialize();
        }

        protected virtual void Update()
        {
            if (gameOver && InputSystem.Instance.Interact())
            {
                OnContinue();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelLoader.Instance.LoadLevel("MainMenu");
            }
        }

        public virtual void Initialize()
        {
            gameOver = false;
            OnMatchStart();
        }

        public virtual void OnMatchStart()
        {
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
        
        protected virtual IEnumerator MatchTimerRoutine()
        {
            float timer = matchTime;
            while (timer > 0)
            {
                hud.timerText.text = timer.ToString();
                yield return new WaitForSeconds(1);
                timer -= 1;
            }

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

        public virtual void OnAfterScoring()
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

            // Empate
            if (matchScore.x == matchScore.y)
            {
                hud.endMatchText.text = "<color=#E15253>EMPATE</color>";
            }
            // left player won
            else if (matchScore.x < matchScore.y)
            {
                hud.endMatchText.text = "<color=#E15253>DERROTA</color>";
            }
            // right player won
            else if (matchScore.x > matchScore.y)
            {
                hud.endMatchText.text = "VITÓRIA";
            }

            int value = Random.Range(10, 50);
            LegendCoin.Instance.EarnLegendCoin(value);
            hud.coinEarnedText.text = "+" + value;
            hud.coinEarnedText.gameObject.SetActive(true);

            if (autoContinueAfterEnd)
            {
                Invoke("OnContinue", continueTimer);
            }
        }

        protected virtual void OnContinue()
        {
            OnMatchStart();
        }

        public virtual void ResetPositions()
        {
            playerOne.position = new Vector3(-6, -0.2f, -2);
            playerTwo.position = new Vector3(6, -0.2f, -2);
            ball.transform.position = new Vector3(0, 3.75f, -2);
        }

        protected virtual void StopBallMovemnt()
        {
            ball.rb.constraints = RigidbodyConstraints.FreezeAll;
            ball.rb.useGravity = false;
            ball.rb.velocity = Vector3.zero;
            ball.rb.angularVelocity = Vector3.zero;
        }

        protected virtual void StartBallMovemnt()
        {
            ball.rb.constraints = RigidbodyConstraints.None;
            ball.rb.constraints = RigidbodyConstraints.FreezeRotation;
            ball.rb.constraints = RigidbodyConstraints.FreezePositionZ;
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

        public virtual IEnumerator WhenScoring()
        {
            Time.timeScale = 0.5f;
            goalAnimtion.SetTrigger("Goal");
            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 1f;
            OnAfterScoring();
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
