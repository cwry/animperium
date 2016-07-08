using UnityEngine;
using System.Collections;


public class CameraFocus : MonoBehaviour {

    /*private GameObject cam {
		get {
			return Camera.main.gameObject;
		}
	}*/

    public GameObject cam;
    public GameObject startingHex;
	
	private float dampen = 8f;
	private float dampenFocus = 2f;

	private bool focusLock = false;
	private bool isMainGrid;

	private Vector3 targetPosition;
	// Use this for initialization
	void Start () {
		StartPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		CameraHandling ();
	}

	private void StartPosition()
	{
        isMainGrid = true;

		CameraJumpInStageMiddle (isMainGrid);
	}

	public void CameraJumpInStageMiddle(bool mainGridBool)
	{
        CameraJump(startingHex);
    }
	public void CameraJump(GameObject target)
	{
        cam.transform.position = target.transform.position;
    }

    

	private void MousePositionHandling()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= Screen.width * 0.95f && mousePosition.y >= Screen.height * 0.95f) {
			targetPosition = cam.transform.position + Vector3.right + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.x >= Screen.width * 0.95f && mousePosition.y <= Screen.height * 0.05f) {
			targetPosition = cam.transform.position + Vector3.right + Vector3.back;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.05f && mousePosition.y <= Screen.height * 0.05f) {
			targetPosition = cam.transform.position + Vector3.left + Vector3.back;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.05f && mousePosition.y >= Screen.height * 0.95f) {
			targetPosition = cam.transform.position + Vector3.left + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.x >= Screen.width * 0.99f) {
			targetPosition = cam.transform.position + Vector3.right;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.01f) {
			targetPosition = cam.transform.position + Vector3.left;
			CameraMove (dampen);
		} else if (mousePosition.y >= Screen.height * 0.99f) {
			targetPosition = cam.transform.position + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.y <= Screen.height * 0.01f) {
			targetPosition = cam.transform.position + Vector3.back;
			CameraMove (dampen);
		}
	}

	private void InputHandling()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			if (isMainGrid) {
				isMainGrid = false;
				CameraJumpInStageMiddle (isMainGrid);
			} else {
				isMainGrid = true;
				CameraJumpInStageMiddle (isMainGrid);
			}
		}
	}

	private void CameraHandling()
	{
		if (focusLock) {
			if (Vector3.Distance (cam.transform.position, targetPosition) > 0.5f) {
				CameraMove (dampenFocus);
			} else {
				focusLock = false;
			}

		} else {
			MousePositionHandling ();
			InputHandling ();
		}
	}

	private void CameraMove(float damp)
	{
		float lerpX = Mathf.Lerp (cam.transform.position.x, targetPosition.x, Time.deltaTime * damp);
		float lerpZ = Mathf.Lerp (cam.transform.position.z, targetPosition.z, Time.deltaTime * damp);
		float y = cam.transform.position.y;
		Vector3 temp = new Vector3 (lerpX, y, lerpZ);
		cam.transform.position = temp;
	}

	private Vector3 GetDirection(Vector3 origin, Vector3 target)
	{
		Vector3 directionTemp = target - origin;
		Vector3.Normalize (directionTemp);
		return directionTemp;
	}

	public void CameraFocusHex(GameObject hex)
	{
		focusLock = true;
        targetPosition = hex.transform.position;
	}

}
