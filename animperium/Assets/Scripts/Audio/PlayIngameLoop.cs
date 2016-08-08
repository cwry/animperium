using UnityEngine;
using System.Collections;

public class PlayIngameLoop : MonoBehaviour {

	void Awake() {
        SoundManager manager = SoundManager.instance;
        manager.StopPlayingSound("mainmenutheme");
        manager.PlaySound("ingameloop");
        manager.PlaySound("bigfight",0.2f);
    }
}
