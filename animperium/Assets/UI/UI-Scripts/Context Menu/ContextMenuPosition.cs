using UnityEngine;
using System.Collections;

public class ContextMenuPosition : MonoBehaviour {

    Vector3 offset;
    public float offsetY = 1.2f;
    Transform t_unit;
    void Awake()
    {
        offset = new Vector3(0,offsetY, 0);
        t_unit = ContextMenuSpawn.currentUnit.transform;
    }
	// Update is called once per frame
	void Update () {
        
        transform.position = Camera.main.WorldToScreenPoint(t_unit.position + offset);
	}

    /*public void Init(GameObject unit)
    {
        t_unit = GUIData.ContextUnit.transform;
    }*/
}
