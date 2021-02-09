using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI Setup")]
    public Text textField;
    public Button nextIndex;

    [Header("Dialogue Setup")]
    public float timeBetweenIndexs = 0.2f;
    public string[] dialogue;

    [Header("End Dialogue Event")]
    public UnityEvent endDialogueEvent = new UnityEvent();

    [Header("Events")]
    public DialogueEvent[] dialogueEvents;

    protected int index = 0;
    protected bool canNext = true;

    void Start()
    {
        nextIndex.onClick.AddListener(() =>
        {
            if (index < dialogue.Length - 1)
            {
                if (canNext) StartCoroutine(NextText());
            }
            else endDialogueEvent.Invoke();
        });

        textField.text = dialogue[index];
    }

    protected void CheckDialogueEvent()
    {
        foreach (var dialogue in dialogueEvents)
        {
            if (dialogue.enableEvent)
            {
                if (dialogue.indexEvent == index)
                {
                    dialogue.unityEvent.Invoke();
                }
            }
        }        
    }

    private IEnumerator NextText()
    {
        canNext = false;

        yield return new WaitForSeconds(timeBetweenIndexs);

        index++;
        textField.text = dialogue[index];

        CheckDialogueEvent();

        canNext = true;
    }

    [System.Serializable]
    public struct DialogueEvent
    {
        public bool enableEvent;
        public int indexEvent;
        public UnityEvent unityEvent;
    }
}
