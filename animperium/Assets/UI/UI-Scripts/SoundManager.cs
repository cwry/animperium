using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct NamedAudioClip
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudioClip[] namedClips;
    public static SoundManager instance;
    private Dictionary<string, AudioClip> soundBible = new Dictionary<string, AudioClip>();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start()
    {
        PlaySound("mainmenutheme");
    }

    public void PlaySound(string sound, Action callback = null)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();

        if (soundBible.ContainsKey(sound))
        {
            a.clip = soundBible[sound];
            a.Play();
        }
        
        StartCoroutine(OnClipEnding(a,callback));
    }

    IEnumerator OnClipEnding(AudioSource a, Action callback)
    {
        while (a.isPlaying){
            yield return 0;
        }
        Destroy(a);
        if (callback != null) callback();
    }

    public void PlaySoundClip(AudioClip c, Action callback = null)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.clip = c;
        a.Play();
        StartCoroutine(OnClipEnding(a, callback));
    }

    void InitSoundBible()
    {
        foreach(NamedAudioClip n in namedClips)
        {
            soundBible.Add(n.name, n.clip);
        }
    }
}
