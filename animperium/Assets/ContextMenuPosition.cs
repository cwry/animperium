using UnityEngine;
using System.Collections;

public class ContextMenuPosition : MonoBehaviour {

    Vector3 initPosition;
	// Use this for initialization
	void Start () {
        initPosition = SelectionManager.selectedTile.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.WorldToScreenPoint(initPosition);
	}
}
