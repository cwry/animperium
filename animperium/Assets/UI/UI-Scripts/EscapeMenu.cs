using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour {

    private bool isActive = false;
    public GameObject escapeMenu;
    
    void Awake() {
        escapeMenu.SetActive(isActive);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TurnOffOnMenu();
        }
    }

    public void TurnOffOnMenu() {
        isActive = !isActive;
        escapeMenu.SetActive(isActive);
        if (!isActive) GUIData.pointerOnGUI = false;
    }
}
