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
    private Dictionary<string, AudioSource> soundsPlaying = new Dictionary<string, AudioSource>();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitSoundBible();
    }
    // Use this for initialization
    void Start()
    {
        PlaySound("mainmenutheme");
    }

    public void PlaySound(string sound, Action callback = null)
    {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        a.volume = 1f;
        if (soundBible.ContainsKey(sound))
        {
            a.clip = soundBible[sound];
            a.Play();
            soundsPlaying.Add(sound, a);
        }
        
        StartCoroutine(OnClipEnding(a,sound,callback));
    }

    public void StopPlayingSound(string sound) {
        soundsPlaying[sound].Stop();
    }
    IEnumerator OnClipEnding(AudioSource a, string name, Action callback)
    {
        while (a.isPlaying){
            yield return 0;
        }
        soundsPlaying.Remove(name);
        Destroy(a);
        if (callback != null) callback();
    }

    void InitSoundBible()
    {
        foreach(NamedAudioClip n in namedClips)
        {
            soundBible.Add(n.name, n.clip);
        }
    }
}
