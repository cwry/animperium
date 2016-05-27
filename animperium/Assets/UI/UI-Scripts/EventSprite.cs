using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventSprite : MonoBehaviour {

    public Sprite normal;
    public Sprite highlighted;
    public Sprite pressed;
    private Image image;

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
}
