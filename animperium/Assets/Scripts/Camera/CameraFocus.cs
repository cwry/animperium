using UnityEngine;
using System.Collections;


public class CameraFocus : MonoBehaviour {

//	T		center camera
//	Space	shift between layers

//JD_Aenderungen
//- rechte Maustaste: Karte ziehen
//- mittlere Maustaste: Karte drehen
//- Mausrad: Zoom (durch Position und FieldOfView)
//- "Horizontal"/"Vertical" (WASD/Pfeiltasten): Kamera ueber Karte bewegen
//- "Rotate View" (YX / Bild rauf & runter): Karte drehen
	// muss im InputManager unter Edit -> Project References -> Input definiert werden

//- Karte verschiebt sich momentan nicht falls die Maus am Rand ist.
//	Stoert beim Testen mit 2 Instanzen auf einem Rechner.

// sollte verschieben vertragen, die Box mag aber keine Rotationen

    /*private GameObject cam {
		get {
			return Camera.main.gameObject;
		}
	}*/

	public GameObject cam;			
	public GameObject startingHex;	// Hex (38|65)
	public GameObject startingHexP1;// Hex (40|91)
	public GameObject startingHexP2;// Hex (28|48)
	private float startAngle = 180f;
	private float startHeight = 17.5f;
	
	private float dampen = 8f;
	private float dampenFocus = 2f;

	private bool focusLock = false;
	private bool isMainGrid;

    private Vector3 offsetSub;
    private Vector3 offsetMain;

	private Vector3 targetPosition;

	// JD_Start
	//für die mittlere Maustaste
	public float dragSpeed = 100.0f;

	// fuer den field of view zoom
	public float minFieldOfViewAngle = 25;
	public float maxFieldOfViewAngle = 30;
	public float minHeight = 10f;
	public float maxHeight = 20f;//Falls identisch, Teilung durch Null möglich

	// fuer die rotation
	public float rotationSpeed = 100f;
	public float minRotAngle = 236;
	public float maxRotAngle = 56;
	private bool rotShift;
	private Vector3 camForward;
	private Vector3 camRight;
	//JD_End

	// fuer die Cameragrenzen
	public float minXBorder = 278;
	public float maxXBorder = 296;
	public float minZBorder = -73;
	public float maxZBorder = -27;

	//fuer die flexibilitaet
	private Vector3 levelPosition;
	private float levelRotationY;
	private Vector3 levelScale;
//	private float sinLvlYneg;
//	private float cosLvlYneg;



	// Use this for initialization
	void Start () {
        offsetSub = GetMiddle(false).transform.position - GetMiddle(true).transform.position;
        offsetMain = GetMiddle(true).transform.position - GetMiddle(false).transform.position;

        //initialize basic movement vectors
		UpdateCamVectors ();

		// www.youtube.com/watch?v=ct7Ke_vH_vU
		Transform levelTransform = GameObject.Find("oberwelt lp").transform;
		levelPosition = levelTransform.position;
		levelRotationY = levelTransform.rotation.eulerAngles.y;
		levelScale = levelTransform.lossyScale;

		minHeight = levelPosition.y + minHeight; // -0 /1
		maxHeight = levelPosition.y + maxHeight; // -0 /1
		Camera.main.fieldOfView = minFieldOfViewAngle + (maxFieldOfViewAngle - minFieldOfViewAngle)
		* (Camera.main.transform.position.y - minHeight) / (maxHeight - minHeight);

		minRotAngle = (minRotAngle + levelRotationY - 146 + 360) % 360;
		maxRotAngle = (maxRotAngle + levelRotationY - 146 + 360) % 360;
		if (minRotAngle > maxRotAngle) {
			rotShift = true;
			minRotAngle -= 180;
			maxRotAngle += 180;
		} else {
			float curY = Camera.main.transform.rotation.eulerAngles.y;
			if (curY < minRotAngle || curY > maxRotAngle)
				Camera.main.transform.RotateAround (cam.transform.position, Vector3.up,180);
		}
//		sinLvlYneg = Mathf.Sin(-levelRotationY);
//		cosLvlYneg = Mathf.Cos(-levelRotationY);

		minXBorder = minXBorder - levelPosition.x;
		maxXBorder = maxXBorder - levelPosition.x;
		minZBorder = minZBorder - levelPosition.z;
		maxZBorder = maxZBorder - levelPosition.z;

		StartPosition ();
    }
	
	// Update is called once per frame
	void Update () {
		CameraHandling ();

		//TEST
		/*
		if (Input.GetKeyDown(KeyCode.O)){
			SCP();
			Debug.Log ("MinX:"+minXBorder+"MaxX:"+maxXBorder+"MinZ:"+minZBorder+"MaxZ:"+maxZBorder);
		}
		*/
	}

	private void StartPosition()
	{
        isMainGrid = true;
		//CameraJumpInStageMiddle (isMainGrid);
		//enshure rotation
		Camera.main.transform.RotateAround(cam.transform.position, Vector3.up,
			startAngle - (Camera.main.transform.rotation.eulerAngles.y - levelRotationY));
		UpdateCamVectors ();
		//enshure height
		Vector3 temp = Camera.main.transform.TransformDirection (Vector3.forward);
		Camera.main.transform.position += temp * (startHeight+levelPosition.y - Camera.main.transform.position.y)/temp.y;
        CameraJumpStart();

    }

    public void CameraJumpStart() {
        //startposition
        if (Data.playerID == 1) {
            CameraJump(startingHexP1);
        }
        if (Data.playerID == 2) {
            CameraJump(startingHexP2);
        }
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
		// JD_Start
		// rotate map
		if (Input.GetMouseButton (2)) {
			float difAngle = Input.GetAxis ("Mouse X") * rotationSpeed * Time.deltaTime;
			difAngle = CheckRotationBoundaries (difAngle);
			if (Mathf.Abs(difAngle) > 0.1f) {
				Camera.main.transform.RotateAround (cam.transform.position, Vector3.up,
					difAngle);
				UpdateCamVectors ();
			}

			//grap map
		} else if (Input.GetMouseButton (1)) {
			Vector3 difPosition = -dragSpeed * Time.deltaTime
				* (Input.GetAxis ("Mouse X") * camRight + Input.GetAxis ("Mouse Y") * camForward);
			difPosition = CheckRectangleBoundaries (difPosition);
			if (Mathf.Abs(difPosition.x) + Mathf.Abs(difPosition.z) > 0.01){
			targetPosition = cam.transform.position + difPosition ;
			CameraMove (dampen);
			}
		}

		// ZOOM - playing around with height and field of view
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			
			if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
				if (Camera.main.transform.position.y < maxHeight) {
					Camera.main.transform.position += Camera.main.transform.TransformDirection (Vector3.back);
					Camera.main.fieldOfView = minFieldOfViewAngle + (maxFieldOfViewAngle - minFieldOfViewAngle)
					* (Camera.main.transform.position.y - minHeight) / (maxHeight - minHeight);
				}
			} else {
				Vector3 temp = Camera.main.transform.position
				               + Camera.main.transform.TransformDirection (Vector3.forward);
				//close to the mountains, thus percisely
				if (temp.y > minHeight) {
					Camera.main.transform.position = temp;
					Camera.main.fieldOfView = minFieldOfViewAngle + (maxFieldOfViewAngle - minFieldOfViewAngle)
						* (Camera.main.transform.position.y - minHeight) / (maxHeight - minHeight);
				}
			}
		}

		/*
		Vector3 mousePosition = Input.mousePosition;
		// small problems in editor
		if  (!Input.GetMouseButton(1)){
			if (mousePosition.x >= Screen.width * 0.95f && mousePosition.y >= Screen.height * 0.95f) {
				targetPosition = cam.transform.position - camLeft + camForward;
				CameraMove (dampen);
			} else if (mousePosition.x >= Screen.width * 0.95f && mousePosition.y <= Screen.height * 0.05f) {
				targetPosition = cam.transform.position - camLeft - camForward;
				CameraMove (dampen);
			} else if (mousePosition.x <= Screen.width * 0.05f && mousePosition.y <= Screen.height * 0.05f) {
				targetPosition = cam.transform.position + camLeft - camForward;
				CameraMove (dampen);
			} else if (mousePosition.x <= Screen.width * 0.05f && mousePosition.y >= Screen.height * 0.95f) {
				targetPosition = cam.transform.position + camLeft + camForward;
				CameraMove (dampen);
			} else if (mousePosition.x >= Screen.width * 0.99f) {
				targetPosition = cam.transform.position - camLeft;
				CameraMove (dampen);
			} else if (mousePosition.x <= Screen.width * 0.01f) {
				targetPosition = cam.transform.position + camLeft;
				CameraMove (dampen);
			} else if (mousePosition.y >= Screen.height * 0.99f) {
				targetPosition = cam.transform.position + camForward;
				CameraMove (dampen);
			} else if (mousePosition.y <= Screen.height * 0.01f) {
				targetPosition = cam.transform.position - camForward;
				CameraMove (dampen);
			}
		}
		*/
	}

	private void InputHandling()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
            CameraJumpStart();
            /*if (isMainGrid) {
				isMainGrid = false;
				CameraJumpInStageMiddle (isMainGrid);
			} else {
				isMainGrid = true;
				CameraJumpInStageMiddle (isMainGrid);
			}*/
        }

		//JD_Start
		// map movement
		if (Input.GetAxis ("Vertical") != 0) {
			//targetPosition = cam.transform.position + camForward * Input.GetAxis ("Vertical");
			//CameraMove (dampen);

			Vector3 difPosition = camForward * Input.GetAxis ("Vertical");
			difPosition = CheckRectangleBoundaries (difPosition);
			if (Mathf.Abs(difPosition.x) + Mathf.Abs(difPosition.z) > 0.01){
				targetPosition = cam.transform.position + difPosition ;
				CameraMove (dampen);
			}

		}
		if (Input.GetAxis ("Horizontal") != 0) {
			//targetPosition = cam.transform.position + camRight * Input.GetAxis ("Horizontal");
			//CameraMove (dampen);

			Vector3 difPosition = camRight * Input.GetAxis ("Horizontal");
			difPosition = CheckRectangleBoundaries (difPosition);
			if (Mathf.Abs(difPosition.x) + Mathf.Abs(difPosition.z) > 0.01){
				targetPosition = cam.transform.position + difPosition ;
				CameraMove (dampen);
			}
		}

		if (Input.GetAxis ("Rotate View") != 0){
			float difAngle = Input.GetAxis ("Rotate View") * rotationSpeed * Time.deltaTime;
			difAngle = CheckRotationBoundaries (difAngle);
			if (Mathf.Abs(difAngle) > 0.1f) {
				Camera.main.transform.RotateAround (cam.transform.position, Vector3.up,
					difAngle);
				UpdateCamVectors ();
			}

		}
		//JD_End

        if (Input.GetKeyDown(KeyCode.Space) && SelectionManager.hoverTile != null)
        {
            /*TileInfo ti = SelectionManager.hoverTile.GetComponent<TileInfo>();
            GridManager otherGrid = ti.grid.isMainGrid ? Data.subGrid : Data.mainGrid;
            TileInfo otherTi = otherGrid.gridData[ti.gridPosition.x, ti.gridPosition.y].GetComponent<TileInfo>();
            Camera.main.GetComponent<CameraFocus>().CameraJump(otherTi.gameObject);*/
            SwapLayer();
        }
	}

    public void SwapLayer() {
        if (Data.isCameraOnMainGrid) {
            cam.transform.position -= offsetSub;
        }
        else {
            cam.transform.position -= offsetMain;
        }
        Data.isCameraOnMainGrid = !Data.isCameraOnMainGrid;
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
		float y = cam.transform.position.y;//or levelPosition.y;//
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
		//Debug.Log("Ziel: "+PS(hex.transform.position));
		targetPosition = hex.transform.position;
	}


    public GameObject GetMiddle(bool mainGridBool)
    {
        int d1;
        int d2;

        if (mainGridBool)
        {
            if (Data.mainGrid.gridWidthInHexes % 2 == 0)
                d1 = Data.mainGrid.gridWidthInHexes / 2;
            else
                d1 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
            if (Data.mainGrid.gridWidthInHexes % 2 == 0)
                d2 = Data.mainGrid.gridWidthInHexes / 2;
            else
                d2 = (Data.mainGrid.gridWidthInHexes - 1) / 2;
            GameObject hexMiddle = Data.mainGrid.gridData[d1, d2];
            return hexMiddle;
        }
        else
        {
            if (Data.subGrid.gridWidthInHexes % 2 == 0)
                d1 = Data.subGrid.gridWidthInHexes / 2;
            else
                d1 = (Data.subGrid.gridWidthInHexes - 1) / 2;
            if (Data.subGrid.gridWidthInHexes % 2 == 0)
                d2 = Data.subGrid.gridWidthInHexes / 2;
            else
                d2 = (Data.subGrid.gridWidthInHexes - 1) / 2;
            GameObject hexMiddle = Data.subGrid.gridData[d1, d2];
            return hexMiddle;
        }
    }

	// checks if cameraGameObject is within boundaries after applying given Positionchange
	public Vector3 CheckRectangleBoundaries(Vector3 positionChange){
		
		Vector3 testPosition = new Vector3 (positionChange.x + cam.transform.position.x - levelPosition.x,
											0, positionChange.z + cam.transform.position.z - levelPosition.z);
		if (Data.isCameraOnMainGrid) testPosition -= offsetSub;
		//testPosition.x = testPosition.x * cosLvlYneg + testPosition.z * sinLvlYneg;
		//testPosition.z = testPosition.x * sinLvlYneg + testPosition.z * cosLvlYneg;

		//Debug.Log(" testPos:"+PS(testPosition)+" camPos: "+PS(cam.transform.position)+" cam in test: "+PS(cam.transform.position-levelPosition));

		if (positionChange.x < 0 && testPosition.x < minXBorder) {
			positionChange.x += Mathf.Abs(minXBorder - testPosition.x);
		} else if (positionChange.x > 0 && testPosition.x > maxXBorder) {
			positionChange.x -= Mathf.Abs(testPosition.x - maxXBorder);
		}
		if (positionChange.z < 0 && testPosition.z < minZBorder) {
			positionChange.z +=Mathf.Abs(minZBorder - testPosition.z);
		} else if (positionChange.z > 0 && testPosition.z > maxZBorder) {
			positionChange.z -= Mathf.Abs(testPosition.z - maxZBorder);
		}

		//Debug.Log ("PosChange"+PS(positionChange));
		return positionChange;
	}

	public float CheckRotationBoundaries(float yAngle){

		float newAngle = Camera.main.transform.rotation.eulerAngles.y + yAngle;
		if (rotShift) newAngle += 180;
		newAngle = (newAngle + 360)% 360;
		if (yAngle < 0 && (newAngle+400)%360 < (minRotAngle+400)%360){
			//Debug.Log ("Bong k");
			return 0;//(yAngle + minRotAngle - newAngle + 360)%360 ;
		}
		if (yAngle > 0 && newAngle > maxRotAngle) {
			//Debug.Log ("Bing g");
			return 0;//(yAngle + maxRotAngle - newAngle + 360)%360 ;
		}

		return yAngle;
	}

	public void UpdateCamVectors(){
			camForward = Camera.main.transform.TransformDirection(Vector3.forward);
			camForward = Vector3.Normalize(new Vector3 (camForward.x, 0, camForward.z));
			camRight = Camera.main.transform.TransformDirection(Vector3.right);
	}

	/*
	//additional testing methods
	public static void SCP(){
		Transform camTrans = Camera.main.transform;
		Vector3 camPar = Camera.main.GetComponentInParent<Transform> ().position;
		Debug.Log ("Cam: "+PS(camTrans.localPosition)+" CamParent: "+ PS(camPar) + "Angle(World): "+PS(camTrans.rotation.eulerAngles));
	}
	// position as a string
	public static string PS(Vector3 pos) {
		return "("+pos.x+"|"+pos.y+"|"+pos.z+")";
	}
	*/
}
