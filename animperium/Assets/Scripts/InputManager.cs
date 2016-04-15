using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
		
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
		if (Input.GetMouseButtonDown (1)) {
			RaycastFromMouse ();
			}
        if (Input.GetMouseButtonDown(0))
        {
            if (SelectionManager.selectedItem != null && SelectionManager.selectedItem.tag != "Unit")
            {
                SelectionManager.Deselect();
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    SelectionManager.SelectObject(hit.collider.gameObject);
                }
            }
        }
	}


	private void RaycastFromMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) {
            Action(hit.collider.gameObject);
		}
		
	}

    public void Action(GameObject g)
    {

        GameObject target = g;
        if (SelectionManager.selectedItem == null)
        {
            switch (GameObjectFilter.TypeOfGameObject(target))
            {
                case "Hex":
                    Camera.main.gameObject.GetComponent<CameraFocus>().CameraFocusHex(target);
                    break;

            }
        }
        else
        {
            SelectionManager.SelectAction(target);
        }
    }
}
