using UnityEngine;
using System.Collections;

public class ContextMenuPosition : MonoBehaviour {

    Vector3 offset;
    public float offsetY = 1.2f;
    void Awake()
    {
        offset = new Vector3(0,offsetY, 0); 
    }
	// Update is called once per frame
	void Update () {
        
        transform.position = Camera.main.WorldToScreenPoint(GUIData.targetTile.transform.position + offset);
	}
}
