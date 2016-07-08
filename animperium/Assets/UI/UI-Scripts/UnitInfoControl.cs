using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitInfoControl : MonoBehaviour {

    
    public GameObject unitInfo;
    GameObject currentUnit;
    public SetDescriptionStats valueText;
    public SetDescriptionStats statusText;
    public Image icon;
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
                valueText.SetValueText(currentUnit);
                statusText.SetDescriptionText(currentUnit);
                icon.sprite = currentUnit.GetComponent<Unit>().icon;
            }
            else
            {
                unitInfo.SetActive(false);
            }
        }
    }
}
