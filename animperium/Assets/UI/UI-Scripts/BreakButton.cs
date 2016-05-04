using UnityEngine;
using System.Collections;

public class BreakButton : MonoBehaviour
{

    public GameObject contextMenu;

    // Use this for initialization
    void Start()
    {
        contextMenu = transform.parent.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        /*
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Ui");
        foreach (GameObject g in arr)
        {
            if (g.name==contextMenu.name)
            {
                Destroy(g);
                GUIData.pointerOnGUI = false;
            }
        }
        */
        Destroy(contextMenu);
        GUIData.pointerOnGUI = false;
    }
}
