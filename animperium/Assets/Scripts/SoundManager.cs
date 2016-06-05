using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip music1;
    private AudioSource player;

	// Use this for initialization
	void Start () {
        player = gameObject.AddComponent<AudioSource>();
        player.volume = 1f;
        player.clip = music1;
        player.Play();
	}
	
	// Update is called once per frame
	void Update () {
	    if(!player.isPlaying)
        {
            player.Play();
        }
	}
}
