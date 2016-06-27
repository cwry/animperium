using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BreakButton : ButtonOnClick
{
    public override void OnClick(PointerEventData data) {
        GameObject menu = GameObject.Find("ContextMenu");
        Destroy(menu);
    }
}
