using UnityEngine;
using System.Collections;

public class SetOnGui : MonoBehaviour {

    
    public void SetOnGUITrue()
    {
        GUIData.pointerOnGUI = true;
        Debug.Log("True");
    }

    public void SetOnGUIFalse()
    {
        GUIData.pointerOnGUI = false;
    }
}
