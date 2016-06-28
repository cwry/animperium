using UnityEngine;
using System.Collections;

public class RotateOnCreate : MonoBehaviour {

    public float speed = 100;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(gameObject.transform.rotation.z < 0f)
        {
            gameObject.transform.Rotate(new Vector3(0f,0f, 1f) * Time.deltaTime * speed);
        }
        else if(gameObject.transform.rotation.z > 0f)
        {
            gameObject.transform.eulerAngles = Vector3.zero;
        }
	}
}
