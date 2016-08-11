using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircleFadeIn : MonoBehaviour {

    Image maskImage;

    public float fadeSpeed = 2.5f;
    bool shouldOpen = true;
    public bool shouldClose = false;
    // Use this for initialization
    void Awake () {
        maskImage = GetComponent<Image>();
        maskImage.fillAmount = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (shouldOpen) {
            if (maskImage.fillAmount < 1f) {
                maskImage.fillAmount += Time.deltaTime + fadeSpeed;
            }
            else shouldOpen = false;
        }

        if (shouldClose) {
            if (maskImage.fillAmount > 0f) {
                maskImage.fillAmount -= Time.deltaTime + fadeSpeed;
            }
            else {
                ContextMenuSpawn.shouldDestroy = true;
            }
        }
	}


}
