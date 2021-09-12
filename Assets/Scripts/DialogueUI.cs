using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;

    void Start()
    {
        GetComponent<TypewriterEffect>().Run("This is a bit of text\nAA", textLabel);
    }
}
