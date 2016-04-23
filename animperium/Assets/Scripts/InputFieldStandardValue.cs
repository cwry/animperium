using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputFieldStandardValue : MonoBehaviour {

    InputField inputField;
	// Use this for initialization
	void Start () {
        inputField = GetComponent<InputField>();
        inputField.text = "7777";
        if(gameObject.name.Contains("IP"))
        {
            inputField.text = "127.0.0.1";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
