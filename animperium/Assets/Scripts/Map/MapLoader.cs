using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

    public GameObject mainGrid;
    public GameObject subGrid;

    void Awake(){
        Data.mainGrid = new GridManager(mainGrid);
        Data.subGrid = new GridManager(subGrid);
    }

}
