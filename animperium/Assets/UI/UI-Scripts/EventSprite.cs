using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventSprite : MonoBehaviour {

    public Sprite normal;
    public Sprite highlighted;
    public Sprite pressed;
    private Image image;
    public Texture2D cursorTexture;
    public Texture2D cursorBasicTexture;

    // Use this for initialization
    void Start () {
        image = gameObject.GetComponent<Image>(); 
	}
	
	public void SwitchToHighlighted()
    {
        image.sprite = highlighted;
    }

    public void SwitchToNormal()
    {
        image.sprite = normal;
    }

    public void SwitchToPressed()
    {
        image.sprite = pressed;
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
