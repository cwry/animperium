using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

    public GameObject hex;
    public Vector3 subOffset;

    void Awake(){
        Data.mainGrid = new GridManager(hex, MapLoadData.mapW, MapLoadData.mapH, true, Vector2.zero);
        Data.subGrid = new GridManager(hex, MapLoadData.mapW, MapLoadData.mapW, false, subOffset);
    }

}
