using UnityEngine;
using System.Collections;

public class GameObjectAction {

	GameObjectFilter filter;
	GameObject gameObject;

	// Use this for initialization
	public GameObjectAction ()
	{
		filter = new GameObjectFilter ();
	}



	public void Action(GameObject g)
	{
		gameObject = g;
		ActionByType ();
		
	}

	public void Action(GameObject[] gA)
	{
		foreach (GameObject g in gA) {
			Action (g);
			}

	}

	private void ActionByType()
	{
		switch(filter.TypeOfGameObject(gameObject))
		{
		case "Hex":
			Camera.main.gameObject.GetComponent<CameraFocus> ().CameraFocusHex (gameObject);
			break;
		}
	}
}
