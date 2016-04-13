using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	GameObjectAction actionManager;

	// Use this for initialization
	void Start () {
		actionManager = new GameObjectAction();
	}
	
	// Update is called once per frame
	void Update () {
		ListenInput ();
	}

	private void ListenInput()
	{
		ListenMouseInput ();
	}

	private void ListenMouseInput()
	{
		if (Input.GetMouseButtonDown (0)) {
			RaycastFromMouse ();
			}
	}


	private void RaycastFromMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
			actionManager.Action (hit.collider.gameObject);
		}
		
	}
}
