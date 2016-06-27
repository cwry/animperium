using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DigButton : ButtonOnClick {

    public override void OnClick(PointerEventData data)
    {
        TeleportMovementManager.dig(SelectionManager.selectedUnit);
    }
}
