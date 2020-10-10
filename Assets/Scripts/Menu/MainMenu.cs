using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public GameObject menu;
    public GameObject equip;

    void Start()
    {
        
    }

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
}
