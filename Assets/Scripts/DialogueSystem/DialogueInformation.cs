using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Dialogue/DialogueInformation")]
public class DialogueInformation : ScriptableObject
{
    [SerializeField] private string characterName;
    public string CharacterName => characterName;
}
