using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetMusicVolume : MonoBehaviour {

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
        soundManager.SetMusicVolume(s.value);
    }

    public void MuteMusic() {
        if(t.isOn) soundManager.SetMusicVolume(0f);
        else soundManager.SetMusicVolume(s.value);
    }
}
