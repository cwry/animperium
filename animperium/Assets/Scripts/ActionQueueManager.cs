using UnityEngine;
using System.Collections;

public class ActionQueueManager : MonoBehaviour {

	void Update () {
        ActionQueue.getInstance().execute();
	}
}
