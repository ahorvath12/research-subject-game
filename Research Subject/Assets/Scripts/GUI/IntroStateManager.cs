using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroStateManager : MonoBehaviour
{
    public static IntroStateManager Instance { get; private set; }
    public bool skipIntroSettings = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
