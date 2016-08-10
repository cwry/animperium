using UnityEngine;
using System.Collections;

public class CameraSwapLayer : MonoBehaviour {

    CameraFocus camFocus;
	// Use this for initialization
	void Awake () {
        camFocus = Camera.main.GetComponent<CameraFocus>();
	}

    public void SwapLayer() {
        camFocus.SwapLayer();
    }
}
