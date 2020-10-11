using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public GameObject menu;
    public GameObject equip;

    public GameObject chapter1;
    public GameObject chapter2;
    public GameObject chapter3;

    public void Menu()
    {
        menu.SetActive(true);
        equip.SetActive(false);
    }

    public void Shop()
    {
        equip.SetActive(true);
        menu.SetActive(false);
    }

    public void PlayGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Chapter1()
    {
        chapter1.SetActive(true);
    }

    public void Chapter2()
    {
        chapter2.SetActive(true);
    }

    public void Chapter3()
    {
        chapter3.SetActive(true);
    }
}
