using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveTextAndSwitchScene : MonoBehaviour {

    public int sceneIndex = 2;
	// Use this for initialization
	void Start () {
        StartCoroutine(WaitLoadNetworkLobby());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            LoadNetworkLobby();
        }
        Vector3 v = gameObject.transform.position + Vector3.up;
        gameObject.transform.position = Vector3.Lerp(transform.position, v, 0.5f);
	}

    IEnumerator WaitLoadNetworkLobby() {
        yield return new WaitForSeconds(37f);
        LoadNetworkLobby();
    }

    public void LoadNetworkLobby() {
        SceneManager.LoadScene(sceneIndex);
    }

}
