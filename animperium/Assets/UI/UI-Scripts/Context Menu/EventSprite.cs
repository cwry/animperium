using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class EventSprite : MonoBehaviour {

    public Color normal;
    public Color highlighted;
    public Color pressed;
    public Color deactivated;
    private Image image;
    public Texture2D cursorTexture;
    public Texture2D cursorBasicTexture;

    // Use this for initialization
    void Start () {
        image = gameObject.GetComponent<Image>(); 
	}
	
	public void SwitchToHighlighted()
    {
        image.color = highlighted;
    }

    public void SwitchToNormal()
    {
        image.color = normal;
    }

    public void SwitchToPressed()
    {
        image.color = pressed;
    }

    public void SwitchToDeactivated()
    {
        image.color = deactivated;
    }

    public void SwitchCursorToTexture()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SwitchCursorBackToBasic()
    {
        Cursor.SetCursor(cursorBasicTexture, Vector2.zero, CursorMode.Auto);
        
    }
}
