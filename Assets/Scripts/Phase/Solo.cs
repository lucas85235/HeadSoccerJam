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

    [Header("Opponents")]
    public CharacteSkin generic;
    public CharacteSkin caio;
    public CharacteSkin bruno;
    public CharacteSkin saitoma;

    void Start()
    {
        GameController.get.MacthEnd += OnSoloEndMatch;
        GameController.get.OnMatchInit();


        var cpuLevel = GameController.get.leftPlayer.GetComponent<Cpu>();

        cpuLevel.cpuLevel = Cpu.CpuLevels.easy;
        cpuLevel.SetCpuLevel();
        GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
            .ChangeSkin(generic);
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
            if (countPhase < 4)
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

        if (countPhase == 0)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.easy;
            cpuLevel.SetCpuLevel();
            GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
                .ChangeSkin(generic);
        }
        else if (countPhase == 1)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.normal;
            cpuLevel.SetCpuLevel();
            GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
                .ChangeSkin(caio);
        }
        else if (countPhase == 2)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.hard;
            cpuLevel.SetCpuLevel();
            GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
                .ChangeSkin(bruno);
        }
        else if (countPhase == 3)
        {
            cpuLevel.cpuLevel = Cpu.CpuLevels.veryHard;
            cpuLevel.SetCpuLevel();
            GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
                .ChangeSkin(saitoma);
        }
    }

    private void OnDestroy() 
    {
        GameController.get.MacthEnd -= OnSoloEndMatch;
    }
}
