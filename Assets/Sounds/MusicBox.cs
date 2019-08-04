using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    static MusicBox instance;
    Dictionary<string, AudioSource> Sounds = new Dictionary<string, AudioSource>();
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Sounds.Add(child.name, child.GetComponent<AudioSource>());
        }
    }

    public static float Timer = 0;
    void Start()
    {
        DontDestroyOnLoad(this);   
    }

    public static void Play(string title)
    {
        if (!instance.Sounds.ContainsKey(title))
        {
            Debug.LogError("Dont see" + title);
        }
        
        instance.Sounds[title].Play();
    }
    
    void Update()
    {
        Timer += Time.deltaTime;
    }
}
