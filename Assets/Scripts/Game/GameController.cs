using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Transform leftPlayer;
    private Transform rightPlayer;
    private Transform ball;

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

    [Header("Debug - No Modify")]
    public bool waitScore = true;
    public bool matchIsEnded = false;

    void Start()
    {
        rightPlayer = GameObject.FindGameObjectWithTag("Player1").transform;
        leftPlayer = GameObject.FindGameObjectWithTag("Player2").transform;
        ball = GameObject.FindGameObjectWithTag("Ball").transform;

        MacthEnd += OnEndMatch;

        OnMatchInit();
        StartCoroutine(MatchTimer());
    }

    public void UpdateScore(Vector2 score)
    {
        matchScore += score;
        leftScoreText.text = matchScore.x.ToString();
        rightScoreText.text = matchScore.y.ToString();
        WhenScoring();
    }

    private void OnMatchInit()
    {
        matchIsEnded = false;

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

    private void WhenScoring()
    {
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
        matchIsEnded = true;
        
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
        if (matchScore.x == matchScore.y)
        {
            endMatchText.text = "EMPATE";
        }
        // left player won
        else if (matchScore.x > matchScore.y)
        {
            endMatchText.text = "VITORIA DO " + leftPlayer.name;
        }
        // right player won
        else if (matchScore.x < matchScore.y)
        {
            endMatchText.text = "VITORIA DO " + rightPlayer.name;
        }

        yield return new WaitForSeconds(2.5f);

        OnMatchInit();
    }

    private void OnDestroy() 
    {
        MacthEnd -= OnEndMatch;
    }
}
