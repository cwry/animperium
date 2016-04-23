using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class NetworkLobbyInput : MonoBehaviour {

    bool isConnected = false;
    public GameObject hostPortInput;
    public GameObject joinPortInput;
    public GameObject joinIPInput;
    public GameObject testModeToggle;

    int hostPort = 7777;
    int joinPort = 7777;
    int n;
    string ip = "127.0.0.1";
    bool isTest = true;
    
    public void SetIP()
    {
        ip = joinIPInput.GetComponent<InputField>().text;
    }

    public void SetIsTest()
    {
        isTest = testModeToggle.GetComponent<Toggle>().isOn;
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
        isConnected = true;
    }

    public void StartGame()
    {
        if (isConnected)
        {
            NetworkData.server.initGame(100, 100, (int)UnityEngine.Random.Range(0, int.MaxValue), isTest);
        }
    }

    public void JoinServer()
    {
        NetworkData.client = new GameClient(ip, joinPort);
        isConnected = true;
    }
}
