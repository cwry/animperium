using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkLobbyInput : MonoBehaviour {
    
    public GameObject hostPortInput;
    public GameObject joinPortInput;
    public GameObject joinIPInput;
    public GameObject startButton;
    public GameObject connectionFields;

    int hostPort = 7777;
    int joinPort = 7777;
    int n;
    string ip = "127.0.0.1";
    
    void Awake() {
        startButton.SetActive(false);
    }
    void Update() {
        if (NetworkData.isConnected) {
            startButton.SetActive(true);
        }
        else {
            startButton.SetActive(false);
        }

        if (NetworkServer.connections.Count == 3) {
            NetworkData.isConnected = true;
        }
        else {
            NetworkData.isConnected = false;
        }
    }
    public void SetIP()
    {
        ip = joinIPInput.GetComponent<InputField>().text;
    }
    
    public void SetHostPort()
    {
       if(Int32.TryParse(hostPortInput.GetComponent<InputField>().text, out n))
        {
            hostPort = n;
        }
       
    }

    public void SetJoinPort()
    {
        if (Int32.TryParse(joinPortInput.GetComponent<InputField>().text, out n))
        {
            joinPort = n;
        }
    }
    public void InitServer()
    {
        NetworkData.server = new GameServer(hostPort);
        NetworkData.client = new GameClient(ip, hostPort);
        connectionFields.SetActive(false);
    }

    public void StartGame()
    {
        if (NetworkData.isConnected)
        {
            NetworkData.server.initGame(100, 100, (int)UnityEngine.Random.Range(0, int.MaxValue), false);
        }
    }

    public void JoinServer()
    {
        if(NetworkData.server != null) {
            NetworkData.server = null;
        }
        NetworkData.client = new GameClient(ip, joinPort);
        connectionFields.SetActive(false);
    }
}
