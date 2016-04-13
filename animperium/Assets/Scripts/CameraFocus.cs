using UnityEngine;
using System.Collections;

public class CameraFocus : MonoBehaviour {

	private GameObject cam {
		get {
			return Camera.main.gameObject;
		}
	}

	private float offSet = 10f;
	private float speed = 5f;
	private float trembelPuffer = 3f;

	private bool focusLock = false;
	private bool isMainGrid;

	private Vector3 targetPosition;
	private Vector3 targetDirection;

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

	private void CameraJumpInStageMiddle(bool mainGridBool)
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
	private void CameraJump(GameObject target)
	{
		cam.transform.position = new Vector3 (target.transform.position.x, offSet, target.transform.position.z - offSet);
	}
	private void MousePositionHandling()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x >= Screen.width * 0.9f && mousePosition.y >= Screen.height * 0.9f) {
			Vector3 direction = Vector3.right + Vector3.forward;
			CameraMove (direction);
		} else if (mousePosition.x >= Screen.width * 0.9f && mousePosition.y <= Screen.height * 0.1f) {
			Vector3 direction = Vector3.right + Vector3.back;
			CameraMove (direction);
		} else if (mousePosition.x <= Screen.width * 0.1f && mousePosition.y <= Screen.height * 0.1f) {
			Vector3 direction = Vector3.left + Vector3.back;
			CameraMove (direction);
		} else if (mousePosition.x <= Screen.width * 0.1f && mousePosition.y >= Screen.height * 0.9f) {
			Vector3 direction = Vector3.left + Vector3.forward;
			CameraMove (direction);
		} else if (mousePosition.x >= Screen.width * 0.9f)
			CameraMove (Vector3.right);
		else if (mousePosition.x <= Screen.width * 0.1f)
			CameraMove (Vector3.left);
		else if (mousePosition.y >= Screen.height * 0.9f)
			CameraMove (Vector3.forward);
		else if (mousePosition.y <= Screen.height * 0.1f)
			CameraMove (Vector3.back);
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
			MoveToFocus ();
		} else {
			MousePositionHandling ();
			InputHandling ();
		}
	}
	private void MoveToFocus()
	{
		if (targetPosition != Vector3.zero && cam.transform.position.x <= targetPosition.x + trembelPuffer && cam.transform.position.z <= targetPosition.z + trembelPuffer
			&& cam.transform.position.x >= targetPosition.x  && cam.transform.position.z >= targetPosition.z ) {
			CameraMove (targetDirection);
		}
		else focusLock = false;
	}

	private void CameraMove(Vector3 direction)
	{
		cam.transform.position += direction * speed * Time.deltaTime;
	}

	public  void CameraFocusHex(GameObject hex)
	{
		focusLock = true;
		Vector3 targetPositionTemp = new Vector3 (hex.transform.position.x, offSet, hex.transform.position.z - offSet);
		Vector3 targetDirectionTemp = targetPositionTemp - cam.transform.position;
		Vector3.Normalize (targetDirectionTemp);
		targetPosition = targetPositionTemp;
		targetDirection = targetDirectionTemp;

	}

}
