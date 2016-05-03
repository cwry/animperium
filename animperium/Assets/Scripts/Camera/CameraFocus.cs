using UnityEngine;
using System.Collections;


public class CameraFocus : MonoBehaviour {

	private GameObject cam {
		get {
			return Camera.main.gameObject;
		}
	}

	private float offSet = 10f;
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
		int d1;
		int d2;

		if (mainGridBool) {
			if (Data.mainGrid.gridWidthInHexes % 2 == 0)
				d1 = Data.mainGrid.gridWidthInHexes / 2;
			else
				d1 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
			if (Data.mainGrid.gridWidthInHexes % 2 == 0)
				d2 = Data.mainGrid.gridWidthInHexes / 2;
			else
				d2 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
			GameObject hexMiddle = Data.mainGrid.gridData [d1, d2];
			CameraJump (hexMiddle);
		}
		else{
			if (Data.subGrid.gridWidthInHexes % 2 == 0)
				d1 = Data.subGrid.gridWidthInHexes / 2;
			else
				d1 = (Data.subGrid.gridWidthInHexes - 1) / 2;
			if (Data.subGrid.gridWidthInHexes % 2 == 0)
				d2 = Data.subGrid.gridWidthInHexes / 2;
			else
				d2 = (Data.subGrid.gridWidthInHexes - 1) / 2;
			GameObject hexMiddle = Data.subGrid.gridData [d1, d2];
			CameraJump (hexMiddle);
		}
	}
	public void CameraJump(GameObject target)
	{
        Debug.Log(Data.mainGrid.gridData[1,1]);
		cam.transform.position = new Vector3 (target.transform.position.x, offSet, target.transform.position.z - offSet);
	}

	private void MousePositionHandling()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= Screen.width * 0.9f && mousePosition.y >= Screen.height * 0.9f) {
			targetPosition = cam.transform.position + Vector3.right + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.x >= Screen.width * 0.9f && mousePosition.y <= Screen.height * 0.1f) {
			targetPosition = cam.transform.position + Vector3.right + Vector3.back;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.1f && mousePosition.y <= Screen.height * 0.1f) {
			targetPosition = cam.transform.position + Vector3.left + Vector3.back;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.1f && mousePosition.y >= Screen.height * 0.9f) {
			targetPosition = cam.transform.position + Vector3.left + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.x >= Screen.width * 0.9f) {
			targetPosition = cam.transform.position + Vector3.right;
			CameraMove (dampen);
		} else if (mousePosition.x <= Screen.width * 0.1f) {
			targetPosition = cam.transform.position + Vector3.left;
			CameraMove (dampen);
		} else if (mousePosition.y >= Screen.height * 0.9f) {
			targetPosition = cam.transform.position + Vector3.forward;
			CameraMove (dampen);
		} else if (mousePosition.y <= Screen.height * 0.1f) {
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
			//MousePositionHandling ();
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
		targetPosition = new Vector3 (hex.transform.position.x, offSet, hex.transform.position.z - offSet);
	}

}
