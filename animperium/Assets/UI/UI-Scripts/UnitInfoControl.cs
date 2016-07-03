using UnityEngine;
using System.Collections;

public class UnitInfoControl : MonoBehaviour {

    
    public GameObject unitInfo;
    GameObject currentUnit;
    public GameObject valueText;
    // Use this for initialization
    void Start()
    {
        unitInfo.SetActive(false);
        currentUnit = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectionManager.selectedUnit != currentUnit)
        {
            currentUnit = SelectionManager.selectedUnit;
            if (currentUnit != null)
            {
                unitInfo.SetActive(true);
                valueText.GetComponent<SetStatusValues>().SetText();
            }
            else
            {
                unitInfo.SetActive(false);
            }
        }
    }
}
