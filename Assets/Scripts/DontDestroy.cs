using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy Instance;
    void Start()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        };

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
