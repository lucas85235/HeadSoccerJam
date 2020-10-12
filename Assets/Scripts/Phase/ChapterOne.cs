using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChapterOne : MonoBehaviour
{
    public GameObject loser;
    public GameObject next;
    public GameObject win;


    [Header("Opponents")]
    public CharacteSkin generic2;

    void Start()
    {
        GameController.get.MacthEnd += OnSoloEndMatch;
        GameController.get.OnMatchInit();


        var cpuLevel = GameController.get.leftPlayer.GetComponent<Cpu>();

        cpuLevel.cpuLevel = Cpu.CpuLevels.easy;
        cpuLevel.SetCpuLevel();
        GameController.get.leftPlayer.GetComponent<ChangeCharactersSprites>()
            .ChangeSkin(generic2);
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
            win.SetActive(true);
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
        next.SetActive(false);
        GameController.get.OnMatchInit();
    }

    private void OnDestroy() 
    {
        GameController.get.MacthEnd -= OnSoloEndMatch;
    }
}
