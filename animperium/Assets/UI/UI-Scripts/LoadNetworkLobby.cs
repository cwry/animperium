using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadNetworkLobby : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(StartLobby());
	}
	
	IEnumerator StartLobby() {
        yield return new WaitForSeconds(12f);
        SceneManager.LoadScene(2);
    }
}
