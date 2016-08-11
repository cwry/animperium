using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowTurnImage : MonoBehaviour {

    public Sprite yourTurn;
    public Sprite nextTurn;
    Image image;
    FadeUI fadeUI;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        fadeUI = GetComponent<FadeUI>();
        TurnManager.onTurnBegin.add<int>((int turnID) => {
            if (Data.isActivePlayer()) {
                image.sprite = yourTurn;
                fadeUI.StartFade();
            }
            else {
                image.sprite = nextTurn;
                fadeUI.StartFade();
            }
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
