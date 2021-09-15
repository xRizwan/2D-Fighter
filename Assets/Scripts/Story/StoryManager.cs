using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public bool hasDefeatedStorm = false;
    public static StoryManager Instance;

    void Start()
    {
        if (Instance != null)
            Destroy(Instance);
        else {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
