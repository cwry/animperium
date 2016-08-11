using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetFactionColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetColor();
	}

    void SetColor() {
        
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        List<Material> materials = new List<Material>();
        foreach(Renderer r in renderer) {
            if(r.sharedMaterial != null) {
                materials.Add(r.sharedMaterial);
            }
        } 
        foreach(Material m in materials) {
            if(m.name == "fraktionsfarbe") {
                if (Data.playerID == 1) m.SetColor("p1", Color.blue);
                else m.SetColor("_DETAIL_MULX2", Color.red);
            }
        }
    }
}
