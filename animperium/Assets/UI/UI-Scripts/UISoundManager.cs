using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISoundManager: MonoBehaviour {


    public AudioClip confirm;
    public AudioClip deconfirm;
    private  AudioSource soundPlayer;
    public Dictionary<string, AudioClip> soundBible = new Dictionary<string, AudioClip>();

	// Use this for initialization
	void Start () {
        soundBible.Add("confirm", confirm);
        soundBible.Add("deconfirm", deconfirm);
        soundPlayer = gameObject.AddComponent<AudioSource>();
        soundPlayer.volume = 1f;
	}

    public  void PlaySound(string sound)
    {
        if (soundBible.ContainsKey(sound))
        {
            soundPlayer.clip = soundBible[sound];
            soundPlayer.Play();
        }
    }
}
