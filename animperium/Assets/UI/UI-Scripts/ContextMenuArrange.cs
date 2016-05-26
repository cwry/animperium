using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ContextMenuArrange : MonoBehaviour {

    public GameObject moveButton;
    public GameObject breakButton;
    public GameObject attackButton;
    private RectTransform panelTransform;

    public int elementCount = 3;
    private float gridSizeHeight;

    private Vector3 firstPosition;
    private Vector3 secondPosition;
    private Vector3 thirdPosition;

    private GameObject firstButton;
    private GameObject secondButton;
    private GameObject thirdButton;
    
    // Use this for initialization
    void Start () {
        panelTransform = gameObject.GetComponent<RectTransform>();
        SetVariables();
        InstantiateButtons();
	}
   
    private void SetVariables()
    {
        gridSizeHeight = (int)panelTransform.sizeDelta.y / elementCount;
        if (elementCount == 3)
        {
            firstPosition = new Vector3(0f, gridSizeHeight, 0f);
            secondPosition = new Vector3(0f, 0f, 0f);
            thirdPosition = new Vector3(0f, -gridSizeHeight, 0f);
        }
        else if(elementCount == 2)
        {
            firstPosition = new Vector3(0f, gridSizeHeight/2, 0f);
            secondPosition = new Vector3(0f, -gridSizeHeight / 2, 0f);
            
        }
    }

    private void InstantiateButtons()
    {
        if (GUIData.targetTile.GetComponent<TileInfo>().unit != null)
        {
            elementCount = 3;
            firstButton = Instantiate(attackButton, transform.TransformPoint(firstPosition), Quaternion.identity) as GameObject;
            firstButton.transform.parent = gameObject.transform;

            secondButton = Instantiate(moveButton, transform.TransformPoint(secondPosition), Quaternion.identity) as GameObject;
            secondButton.transform.parent = gameObject.transform;

            secondButton = Instantiate(breakButton, transform.TransformPoint(thirdPosition), Quaternion.identity) as GameObject;
            secondButton.transform.parent = gameObject.transform;
        }
        else
        {
            elementCount = 2;
            firstButton = Instantiate(moveButton, transform.TransformPoint(firstPosition), Quaternion.identity) as GameObject;
            firstButton.transform.parent = gameObject.transform;

            secondButton = Instantiate(breakButton, transform.TransformPoint(secondPosition), Quaternion.identity) as GameObject;
            secondButton.transform.parent = gameObject.transform;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
