using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterSeconds : MonoBehaviour
{
    public float timeToHide;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Hide", timeToHide);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

}
