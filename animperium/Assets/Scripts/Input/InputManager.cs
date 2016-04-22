using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	void Update () {
        GameObject tileHover = getTileHover();
        SelectionManager.hoverTile = tileHover;
        setSelection(tileHover);
	}

    private GameObject getTileHover(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            return hit.collider.gameObject;
        }
        return null;
    }

    private void setSelection(GameObject hover){
        if (Input.GetMouseButtonDown(0)){
            SelectionManager.selectedTile = hover;
            if (hover == null) return;
            SelectionManager.selectedUnit = hover.GetComponent<TileInfo>().unit;
        }
    }

    /*private void ListenInput()
    {
        ListenMouseInput();
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
    }*/
}
