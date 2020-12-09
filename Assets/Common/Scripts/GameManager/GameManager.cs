using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MadeInHouse
{
    public class GameManager : MonoBehaviour
    {
        public Action OnEndMatch;

        [Header("Players")]
        public Transform playerOne;
        public Transform playerTwo;
        public Transform ball;

        protected Vector2 matchScore;
        protected bool gameOver;
        public bool GameOver
        {
            get => gameOver;
            set
            {
                if (value && OnEndMatch != null) OnEndMatch();
                gameOver = value;
            }
        }

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

            Initialize();
        }

        public virtual void Initialize()
        {
            GameOver = false;
            OnMatchStart();
        }

        public virtual void OnMatchStart()
        {
            matchScore = new Vector2(0, 0);
            ResetPositions();
        }

        public virtual void OnAfterScoring()
        {
            ResetPositions();
        }

        public virtual void ResetPositions()
        {
            playerOne.position = new Vector3(-6, -0.2f, -2);
            playerTwo.position = new Vector3(6, -0.2f, -2);
            ball.position = new Vector3(0, 3.75f, -2);
        }

        public virtual void UpdateScore(Vector2 score)
        {
            if (score == null)
            {
                matchScore = new Vector2(0, 0);
                return;
            }

            matchScore += score;

            Players player;
            if (score.x > 0)
            {
                player = Players.one;
            }
            else player = Players.two;

            StartCoroutine(WhenScoring(player));
        }

        public virtual IEnumerator WhenScoring(Players whoScored)
        {
            yield return null;
            Debug.Log("GOAL " + whoScored);
        }

        /// <summary> Reset After Destroy </summary>
        private void OnDestroy()
        {
            OnEndMatch = null;
        }
    }

    public enum Players
    {
        one,
        two
    }

    public enum WhoWon
    {
        deadheat,
        playerOneWon,
        playerTwoWon,
    }
}
