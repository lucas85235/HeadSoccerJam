using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour
{
    [Header("Setup UI")]
    public Text textField;
    public Button next;
    public GameObject zico;
    public GameObject backgound1;
    public GameObject backgound2;
    public Image fade;
    public float fadeSpeed;

    [Header("Texts")]
    public string[] dialogue;
    public int index = 0;

    private bool canNext = true;

    void Start()
    {
        next.onClick.AddListener(() =>
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

        if (index == 35)
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                fade.color = new Color(0, 0, 0, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            zico.SetActive(true);
            backgound1.SetActive(false);
            backgound2.SetActive(true);
            
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                fade.color = new Color(0, 0, 0, i);
                yield return new WaitForSeconds(Time.deltaTime);
            }


        }

        yield return new WaitForSeconds(0.2f);
        index++;
        textField.text = dialogue[index];

        // if (index == 35)
        // {
        //     for (float i = 0; i <= 1; i += Time.deltaTime)
        //     {
        //         // set color with i as alpha
        //         fade.color = new Color(0, 0, 0, i);
        //         yield return new WaitForSeconds(Time.deltaTime);
        //     }

        //     // loop over 1 second backwards
        //     for (float i = 1; i >= 0; i -= Time.deltaTime)
        //     {
        //         // set color with i as alpha
        //         fade.color = new Color(0, 0, 0, i);
        //         yield return new WaitForSeconds(Time.deltaTime);
        //     }
        // }
        // else if (index == 37)
        // {
        //     zico.SetActive(true);
        //     backgound1.SetActive(false);
        //     backgound2.SetActive(true);
        // }

        canNext = true;
    }
}
