using UnityEngine;
using System.Collections;

public class DigButton : MonoBehaviour {

    public void OnClick()
    {
        TeleportMovementManager.dig(SelectionManager.selectedUnit);
    }
}
