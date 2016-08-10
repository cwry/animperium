using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesText : MonoBehaviour {

    public Text wood;
    public Text iron;
    public Text stone;
    public Text gold;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        wood.text = Data.wood.ToString();
        iron.text = Data.iron.ToString();
        stone.text = Data.stone.ToString();
        gold.text = Data.gold.ToString();
    }
    
}
