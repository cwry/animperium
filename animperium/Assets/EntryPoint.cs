using UnityEngine;
using System.Collections;

public class EntryPoint : MonoBehaviour {

    public GameObject hex;
    public int gridWidth;
    public int gridHeight;
    public Vector3 subOffset;

    // Use this for initialization
    void Awake(){
        Debug.Log(hex);
        Data.mainGrid = new GridManager(hex, gridWidth, gridHeight, true, Vector2.zero);
        Data.subGrid = new GridManager(hex, gridWidth, gridHeight, false, subOffset);
    }

}
