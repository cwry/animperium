using UnityEngine;
using System.Collections;

public class DefaultUnit : MonoBehaviour {

	void Awake(){
        Data.units.Add(GetComponent<Unit>().unitID, gameObject);
    }
}
