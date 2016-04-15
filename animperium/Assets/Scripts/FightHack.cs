using UnityEngine;
using System.Collections;

public class FightHack : MonoBehaviour {

    
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (SelectionManager.selectedTile != null)
        {
            if (Input.GetKeyDown(KeyCode.A) && SelectionManager.selectedTile.GetComponent<TileInfo>().unit != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<TileInfo>().unit != null)
                    {
                        SelectionManager.selectedTile.GetComponent<TileInfo>().unit.GetComponent<Unit>().functions.attack(SelectionManager.selectedTile.GetComponent<TileInfo>().unit, hit.collider.gameObject.GetComponent<TileInfo>().unit);
                    }
                }

            }
        }
	}
}
