using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeSpriteColor : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public Color normal;
    public Color highlighted;
    public Color pressed;
    public Color deactivated;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normal;
    }

    public void SwitchToHighlighted() {
        spriteRenderer.color = highlighted;
    }

    public void SwitchToNormal() {
        spriteRenderer.color = normal;
    }

    public void SwitchToPressed() {
        spriteRenderer.color = pressed;
    }

    public void SwitchToDeactivated() {
        spriteRenderer.color = deactivated;
    }
}
