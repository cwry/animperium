using UnityEngine;
using System.Collections;

public class CameraMoveOnRightclick : MonoBehaviour {
	void Update () {
        if (Input.GetMouseButtonDown(1) && SelectionManager.hoverTile != null){
            Camera.main.gameObject.GetComponent<CameraFocus>().CameraFocusHex(SelectionManager.hoverTile);
        }
	}
}
