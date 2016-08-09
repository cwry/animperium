using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetSoundEffectVolume : MonoBehaviour {
    private SoundManager soundManager;
    public GameObject slider;
    public GameObject toggle;
    private Slider s;
    private Toggle t;

    void Awake() {
        soundManager = SoundManager.instance;
        s = slider.GetComponent<Slider>();
        t = toggle.GetComponent<Toggle>();
    }

    public void SetVolume() {
        soundManager.SetSoundEffectVolume(s.value);
    }

    public void MuteSoundEffect() {
        if (t.isOn) soundManager.SetSoundEffectVolume(0f);
        else soundManager.SetSoundEffectVolume(s.value);
    }
}
