using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    public DialogueActivator dialogueActivator;
    public DialogueObject lostDialogue;

    public void UpdateStory(){
        dialogueActivator.UpdateDialogueObject(lostDialogue);
    }

    void Start()
    {
        if (StoryManager.Instance.hasDefeatedStorm) {
            UpdateStory();
        }
    }
}
