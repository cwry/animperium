using UnityEngine;
using System.Collections;

public class UnitInfoControl : MonoBehaviour {

    
    public GameObject unitInfo;
    GameObject currentUnit;
    public SetStatusValues valueText;
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
                valueText.SetText(currentUnit);
            }
            else
            {
                unitInfo.SetActive(false);
            }
        }
    }
}
