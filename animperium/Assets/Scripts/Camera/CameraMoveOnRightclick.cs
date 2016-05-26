using UnityEngine;
using System.Collections;

public class CameraMoveOnRightclick : MonoBehaviour {
	void Update () {
        if (Input.GetMouseButtonDown(1) && SelectionManager.hoverTile != null){
            Camera.main.gameObject.transform.GetComponentInParent<CameraFocus>().CameraFocusHex(SelectionManager.hoverTile);
        }
	}
}
