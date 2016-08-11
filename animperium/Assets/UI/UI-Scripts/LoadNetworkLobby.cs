using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadNetworkLobby : MonoBehaviour {

    public int sceneIndex = 2;
    // Use this for initialization
    void Start () {
        StartCoroutine(StartLobby());
	}
	
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadLobby();
        }
    }
	IEnumerator StartLobby() {
        yield return new WaitForSeconds(12f);
        LoadLobby();
    }
    void LoadLobby() {
        SceneManager.LoadScene(sceneIndex);
    }
}
