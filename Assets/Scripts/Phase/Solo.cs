using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Solo : MonoBehaviour
{
    public GameObject loser;
    public GameObject next;
    public GameObject win;

    public int countPhase = 0;

    void Start()
    {
        GameController.get.MacthEnd += OnSoloEndMatch;
        GameController.get.OnMatchInit();
    }

    private void OnSoloEndMatch(GameController.Match match)
    {
        StartCoroutine(OnEndMatchRoutine(match));
    }

    private IEnumerator OnEndMatchRoutine(GameController.Match match)
    {
        yield return new WaitForSeconds(2.5f);
        GameController.get.endMatchText.gameObject.SetActive(false);

        // Empate
        if (match == GameController.Match.empate)
        {
            loser.SetActive(true);
        }
        // left player won
        else if (match == GameController.Match.leftWon)
        {
            loser.SetActive(true);
        }
        // right player won
        else if (match == GameController.Match.rightWon)
        {
            if (countPhase < 8)
            {
                next.SetActive(true);
            }
            else win.SetActive(true);
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Continue()
    {
        countPhase++;
        next.SetActive(false);
        GameController.get.OnMatchInit();

        var cpuLevel = GameController.get.leftPlayer.GetComponent<Cpu>();

        if (countPhase < 2)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.easy;
            cpuLevel.SetCpuLevel();            
        }
        else if (countPhase < 4)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.normal;
            cpuLevel.SetCpuLevel();            
        }
        else if (countPhase < 6)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.hard;
            cpuLevel.SetCpuLevel();            
        }
        else if (countPhase < 8)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.veryHard;
            cpuLevel.SetCpuLevel();            
        }
    }

    private void OnDestroy() 
    {
        GameController.get.MacthEnd -= OnSoloEndMatch;
    }
}
