using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject nameBox;

    public bool IsOpen {get; private set;}
    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {

        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            
            yield return RunTypingEffect(dialogue);
            textLabel.text = dialogue;
            
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }


        if (dialogueObject.HasResponses) {
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            textLabel.text = string.Empty;
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else {
            CloseDialogueBox();
        }

    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    public void OpenNameBox(DialogueInformation dialogueInformation)
    {
        if (dialogueInformation != null) {
            nameBox.GetComponentInChildren<Text>().text = dialogueInformation.CharacterName;
            nameBox.SetActive(true);
        } else nameBox.SetActive(false);
    }
}
