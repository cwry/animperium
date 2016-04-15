using UnityEngine;
using System.Collections;

public class FightHack : MonoBehaviour {

    
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectionManager.selectedUnit != null)
        {
            if (Input.GetKeyDown(KeyCode.A) && SelectionManager.selectedUnit != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<TileInfo>().unit != null)
                    {
                        SelectionManager.selectedUnit.GetComponent<Unit>().functions.attack(SelectionManager.selectedUnit, hit.collider.gameObject.GetComponent<TileInfo>().unit);
                    }
                }

            }
        }
	}
}
