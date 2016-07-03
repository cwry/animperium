using UnityEngine;
using System.Collections;

public class ContextMenuPosition : MonoBehaviour {
    
    void Awake()
    {

    }
	// Update is called once per frame
	void Update () {
        
        transform.position = Camera.main.WorldToScreenPoint(GUIData.targetTile.transform.position);
	}
}
