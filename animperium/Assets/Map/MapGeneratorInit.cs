using UnityEngine;
using System.Collections;

public class MapGeneratorInit : MonoBehaviour {

    public int w = 50;
    public int h = 50;
    public GameObject hex;

	void Awake () {
        new GridManager(hex, w, h, true);
    }
}
