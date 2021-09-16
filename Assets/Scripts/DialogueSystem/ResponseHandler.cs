using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;
    private DialogueUI dialogueUI;
    private ResponseEvent[] responseEvents;
    private int chosenButton;
    private bool isDisplayingResponses;
    private Coroutine chosingCoroutine;

    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    List<GameObject> tempResponseButtons = new List<GameObject>();

    public void ShowResponses(Response[] responses)
    {
        // float responseBoxHeight = 0;
        isDisplayingResponses = true;

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i;

            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));
            
            tempResponseButtons.Add(responseButton);

            // responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }


        // responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
        
        chosenButton = 0;
        HandleSelect();
        chosingCoroutine = StartCoroutine(Chose(responses));
    }

    IEnumerator Chose(Response[] responses)
    {

        while (isDisplayingResponses) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                chosenButton++;
                HandleSelect();
            }

            yield return null;
            if (Input.GetKeyDown(KeyCode.Space)) break;
        }

        OnPickedResponse(responses[chosenButton], chosenButton);

    }

    public void OnPickedResponse(Response response, int responseIndex)
    {
        if (chosingCoroutine != null) StopCoroutine(chosingCoroutine);
        isDisplayingResponses = false;
        responseBox.gameObject.SetActive(false);

        Debug.Log("here 3 " + "   " + response + "  :  " + responseEvents.Length + "   :  " + responseIndex);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();

        if (responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null;
    
        if (response.DialogueObject)
        {
            dialogueUI.ShowDialogue(response.DialogueObject);
        } else 
        {
            dialogueUI.CloseDialogueBox();
        }
    }
    void SetSelectedColor(TMP_Text text)
    {
        text.color = Color.blue;
    }

    void SetDisabledColor(TMP_Text text)
    {
        text.color = Color.black;
    }

    void HandleSelect()
    {
        for(int i = 0; i < tempResponseButtons.Count; i++)
        {
            if (i == chosenButton) tempResponseButtons[i].GetComponent<TMP_Text>().color = Color.blue;
            else tempResponseButtons[i].GetComponent<TMP_Text>().color = Color.black;           
        }
    }
}

