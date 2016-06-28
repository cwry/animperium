using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DigButton : MonoBehaviour {

    public  void OnClick(PointerEventData data)
    {
        TeleportMovementManager.dig(SelectionManager.selectedUnit);
    }
}
