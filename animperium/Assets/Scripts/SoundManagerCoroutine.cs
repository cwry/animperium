using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SoundManagerCoroutine: MonoBehaviour {


    public AudioClip musicMenu;
    public AudioClip musicGame;
    CoroutineManager coroutineManager;
    private int currentSceneIndex;
    private AudioSource musicPlayer;
    
    void Awake() {
        DontDestroyOnLoad(gameObject);  //we need this object in every scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        musicPlayer = new AudioSource();
        coroutineManager = gameObject.AddComponent<CoroutineManager>();
        coroutineManager.Add(StartCoroutine(PlaySound(musicMenu)), "playMainMusic");
    }
    //is true when the scene has been changed
    public bool IsSceneChanged(){
        if(SceneManager.GetActiveScene().buildIndex != currentSceneIndex){
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            return true;
        }
        return false;
    }
   
    //Enum playing an audio clip and stays alive for its length in seconds
    public IEnumerator PlaySound(AudioClip c){
        AudioSource s = new AudioSource();
        s.clip = c;
        s.Play();
        yield return new WaitForSeconds(s.clip.length);
        yield return null;
    }
}
