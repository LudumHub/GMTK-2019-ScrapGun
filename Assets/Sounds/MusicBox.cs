using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    MusicBox instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public static float Timer = 0;
    void Start()
    {
        DontDestroyOnLoad(this);   
    }

    void Update()
    {
        Timer += Time.deltaTime;
    }
}
