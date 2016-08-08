using UnityEngine;
using System.Collections;

public class PlayIngameLoop : MonoBehaviour {

    SoundManager manager;
    void Awake() {
        manager = SoundManager.instance;
        manager.StopPlayingSound("mainmenutheme");
        manager.PlaySound("ingameloop");
    }

    void Update() {
        if (!manager.isPlaying("ingameloop")) manager.PlaySound("ingameloop", SoundManager.musicVolume);
    }
}
