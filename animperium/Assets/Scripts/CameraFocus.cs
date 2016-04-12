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

	// Use this for initialization
	void Start () {
		StartPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		MousePositionHandling ();
	}

	private void StartPosition()
	{
		int d1;
		int d2;
		if(Data.mainGrid.gridWidthInHexes % 2 == 0)  d1 = Data.mainGrid.gridWidthInHexes / 2;
		else  d1 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
		if(Data.mainGrid.gridWidthInHexes % 2 == 0)  d2 = Data.mainGrid.gridWidthInHexes / 2;
		else  d2 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
		GameObject hexMiddle = Data.mainGrid.gridData [d1, d2];
		cam.transform.position = new Vector3 (hexMiddle.transform.position.x, offSet, hexMiddle.transform.position.z - offSet);
	}

	private void MousePositionHandling()
	{
		Vector3 mousePosition = Input.mousePosition;

		if (mousePosition.x >= Screen.width * 0.9f)
			CameraMove (Vector3.right);
		else if (mousePosition.x <= Screen.width * 0.1f)
			CameraMove (Vector3.left);
		else if (mousePosition.y >= Screen.height * 0.9f)
			CameraMove (Vector3.forward);
		else if (mousePosition.y <= Screen.height * 0.1f)
			CameraMove (Vector3.back);
	}

	private void CameraMove(Vector3 direction)
	{
		cam.transform.position += direction * speed * Time.deltaTime;

	}

	public  void cameraFocusHex(GameObject hex)
	{
		cam.transform.position = new Vector3 (hex.transform.position.x, offSet, hex.transform.position.z - offSet);
	}

}
