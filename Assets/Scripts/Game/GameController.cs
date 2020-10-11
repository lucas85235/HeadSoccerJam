﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform leftPlayer { get; private set; }
    public Transform rightPlayer { get; private set; }
    public Transform ball { get; private set; }

    // left vs right
    private Vector2 matchScore = new Vector2(0, 0);

    // Define event after end of match
    public enum Match { empate, leftWon, rightWon }
    public Action<Match> MacthEnd;

    [Header("Setup")]
    public Text timerText;
    public Text leftScoreText;
    public Text rightScoreText;
    public Text endMatchText;

    [Header("Match Settigs")]
    public float matchTime = 90f;

    [Header("Audios")]
    public AudioClip startWhistle;
    public AudioClip restartWhistle;
    public AudioClip endWhistle;
    public AudioClip afterScore;

    [Header("Debug - No Modify")]
    public bool waitScore = true;
    public bool matchIsEnded = false;
    public bool isStartMatch = false;

    public static GameController get;

    void Awake() 
    {
        get = this;

        rightPlayer = GameObject.FindGameObjectWithTag("Player1").transform;
        leftPlayer = GameObject.FindGameObjectWithTag("Player2").transform;
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
    }

    void Start()
    {
        MacthEnd += OnEndMatch;
    }

    public void UpdateScore(Vector2 score)
    {
        matchScore += score;
        leftScoreText.text = matchScore.x.ToString();
        rightScoreText.text = matchScore.y.ToString();
        StartCoroutine(WhenScoring());
    }

    public void OnMatchInit()
    {
        Time.timeScale = 1f;
        matchIsEnded = false;
        isStartMatch = false;

        // Reset Scores
        matchScore = new Vector2(0, 0);
        UpdateScore(Vector2.zero);

        // Reset Positions
        leftPlayer.position = new Vector3(-6, -0.2f, -2);
        rightPlayer.position = new Vector3(6, -0.2f, -2);

        // Disable EndText
        endMatchText.gameObject.SetActive(false);

        // Start timer count
        waitScore = true;
        StartCoroutine(MatchTimer());
    }

    private IEnumerator WhenScoring()
    {
        if (isStartMatch)
        {
            Time.timeScale = 0.5f;
            AudioSource.PlayClipAtPoint(afterScore, Camera.main.transform.position);
            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 1f;
        }

        // Reset Positions
        leftPlayer.position = new Vector3(-6, -0.2f, -2);
        rightPlayer.position = new Vector3(6, -0.2f, -2);
        ball.position = new Vector3(0, 3.75f, -2);

        // Stop ball movement
        var ballRb = ball.GetComponent<Rigidbody>();
        ballRb.useGravity = false;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    
        waitScore = true;
    }

    private IEnumerator MatchTimer()
    {
        float timer = matchTime;

        // Count time
        while (timer > 0)
        {
            // Pause afte start and scoring
            if (waitScore)
            {
                // Start Match
                if (!isStartMatch)
                {
                    AudioSource.PlayClipAtPoint(startWhistle, Camera.main.transform.position);
                    isStartMatch = true;
                }
                // Restart Match
                else AudioSource.PlayClipAtPoint(restartWhistle, Camera.main.transform.position);
                
                yield return new WaitForSeconds(1);
                ball.GetComponent<Rigidbody>().useGravity = true;
                waitScore = false;
            }
            
            // count time of match
            yield return new WaitForSeconds(1f);
            timer -= 1;
            timerText.text = timer.ToString();
        }

        Debug.Log("Match is Finish");
        AudioSource.PlayClipAtPoint(endWhistle, Camera.main.transform.position);
        matchIsEnded = true;
        Time.timeScale = 0.8f;
        
        endMatchText.text = "Fim da Parida";
        endMatchText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.25f);

        // Empate
        if (matchScore.x == matchScore.y) MacthEnd(Match.empate);
        // left player won
        else if (matchScore.x > matchScore.y) MacthEnd(Match.leftWon);
        // right player won
        else if (matchScore.x < matchScore.y) MacthEnd(Match.rightWon);
    }

    private void OnEndMatch(Match match)
    {
        StartCoroutine(OnEndMatchRoutine(match));
    }

    private IEnumerator OnEndMatchRoutine(Match match)
    {
        // Empate
        if (match == Match.empate)
        {
            endMatchText.text = "EMPATE";
        }
        // left player won
        else if (match == Match.leftWon)
        {
            endMatchText.text = "VITORIA DO " + leftPlayer.name;
        }
        // right player won
        else if (match == Match.rightWon)
        {
            endMatchText.text = "VITORIA DO " + rightPlayer.name;
        }

        yield return new WaitForSeconds(2.5f);
    }

    private void OnDestroy() 
    {
        MacthEnd -= OnEndMatch;
    }
}
