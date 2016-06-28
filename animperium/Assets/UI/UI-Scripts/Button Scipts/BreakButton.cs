using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BreakButton : MonoBehaviour
{
    public void OnClick(PointerEventData data) {
        GameObject menu = GameObject.Find("ContextMenu");
        Destroy(menu);
    }
}
