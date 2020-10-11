using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour
{
    [Header("Setup UI")]
    public Text textField;
    public Button next;

    [Header("Texts")]
    public string[] dialogue;
    public int index = 0;

    private bool canNext = true;

    void Start()
    {
        next.onClick.AddListener( () => 
        {
            if (index < dialogue.Length - 1)
            { 
                if (canNext) StartCoroutine(NextText());
            }
            else MainMenu.get.LoadChapter1();
        });

        textField.text = dialogue[index];
    }

    private IEnumerator NextText()
    {
        canNext = false;
        yield return new WaitForSeconds(0.2f);
        index++;
        textField.text = dialogue[index];
        canNext = true;
    }
}
