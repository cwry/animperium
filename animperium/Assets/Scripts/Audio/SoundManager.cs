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
        public bool isMusic;
    }

    public NamedAudioClip[] namedClips;
    public static SoundManager instance;
    public static float musicVolume = 1f;
    public static float effectVolume = 1f;
    public Dictionary<string, AudioClip> soundBible = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioSource> soundsPlaying = new Dictionary<string, AudioSource>();

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitSoundBible();
    }
    // Use this for initialization
    void Start() {
        PlaySound("mainmenutheme", SoundManager.musicVolume);
    }
    
    public void PlaySound(string sound, float volume = 1f, Action callback = null) {
        AudioSource a = gameObject.AddComponent<AudioSource>();
        if (soundBible.ContainsKey(sound)) {
            a.clip = soundBible[sound];
            a.Play();
            a.volume = volume;
            soundsPlaying.Add(sound, a);
        }
        
        StartCoroutine(OnClipEnding(a,sound,callback));
    }
    public bool isPlaying(string sound) {
        if (soundsPlaying.ContainsKey(sound)) return true;
        else return false;
    }
    public void StopPlayingSound(string sound) {
        if (soundsPlaying.ContainsKey(sound)) {
            soundsPlaying[sound].Stop();
        }
    }
    public void SetVolume(string sound, float volume) {
        if (soundsPlaying.ContainsKey(sound)) {
            soundsPlaying[sound].volume = volume;
        }
    }
    public AudioSource GetAudioSource(string sound) {
        if (soundsPlaying.ContainsKey(sound)) {
            return soundsPlaying[sound];
        }
        return null;
    }
    IEnumerator OnClipEnding(AudioSource a, string name, Action callback) {
        while (a.isPlaying){
            yield return 0;
        }
        soundsPlaying.Remove(name);
        Destroy(a);
        if (callback != null) callback();
    }
    void InitSoundBible() {
        foreach(NamedAudioClip n in namedClips) {
            soundBible.Add(n.name, n.clip);
        }
    }
    public void SetMusicVolume(float volume) {
        musicVolume = volume;
        foreach(NamedAudioClip n in namedClips) {
            if (n.isMusic) {
                SetVolume(n.name, musicVolume);
            }
        }
    }
    public void SetSoundEffectVolume(float volume) {
        effectVolume = volume;
        foreach (NamedAudioClip n in namedClips) {
            if (!n.isMusic) {
                SetVolume(n.name, effectVolume);
            }
        }
    }
}
