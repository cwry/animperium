using UnityEngine;
using System;

//AbilityManager.listAbilities(Selected unit);// returnt string array with ability ids
//AbilityManager.checkRange(unit, "Melee"); //gameobject array with all possible targets
//AbilityManager.useAbility(unit, "Melee", Vec2i target, ismaingrid) // löst ability melee aus
class ContextMenuControl : MonoBehaviour
{
    int slot = 0;
    public GameObject[] slots = new  GameObject[5];
    public GameObject attackButton;
    public GameObject moveButton;
    public GameObject digButton;
    public ContextMenuSpawn spawn;
    // public System.Collections.Generic.Dictionary<GameObject, GameObject> buttonDic;

        void Start()
    {
        spawn = GameObject.FindObjectOfType<ContextMenuSpawn>();
    }
    public void AddButton(string ability)// add button to existing points
    {
            if (slot < slots.Length)
            {
                Debug.Log("Sollte instanziert werden");
                GameObject g = Instantiate(attackButton,slots[slot].GetComponent<Transform>().position, Quaternion.identity) as GameObject;
                g.transform.SetParent(slots[slot].transform.parent);
                Destroy(slots[slot]);
                slots[slot] = g;
                slots[slot].AddComponent<ButtonComponent>();
                slots[slot].GetComponent<ButtonComponent>().ability = ability;
                //slots[slot].GetComponent<ButtonComponent>().button.SetFunction(() => AbilityManager.useAbility(SelectionManager;.selectedUnit, ability, SelectionManager.selectedTarget.GetComponent<TileInfo>().gridPosition, SelectionManager.selectedTarget.GetComponent<TileInfo>().grid.isMainGrid))
            }
                
        slot++;
    }

    public void AddMoveButton()
    {
        GameObject g = Instantiate(moveButton, slots[slot].GetComponent<Transform>().position, Quaternion.identity) as GameObject;
        g.transform.SetParent(slots[slot].transform.parent);
        Destroy(slots[slot]);
        slots[slot] = g;
        slots[slot].AddComponent<ButtonComponent>();
        
    }
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0) 
            && !GUIData.pointerOnGUI 
            && GUIData.canSelectTarget 
            && Data.isEndTurnPossible() 
            && GUIData.activeButton != null)
        {
            GameObject[] s = AbilityManager.checkRange(SelectionManager.selectedUnit, GUIData.activeButton.GetComponent<ButtonComponent>().ability);
            Debug.Log(s.ToString());
            foreach(GameObject g in s)
            {
                if (g==SelectionManager.selectedTile)
                {
                 AbilityManager.useAbility(spawn.currentUnit,GUIData.activeButton.GetComponent<ButtonComponent>().ability, SelectionManager.selectedTile.GetComponent<TileInfo>().gridPosition, SelectionManager.selectedTile.GetComponent<TileInfo>().grid.isMainGrid);
                 GUIData.activeButton = null;
                 GUIData.canSelectTarget = false;
                }
            }
           
        }
        //if (Input.GetMouseButtonDown(0) && !GUIData.pointerOnGUI && GUIData.canSelectTarget //MoveButtonAbfrage
        //   && Data.isEndTurnPossible()
        //   && SelectionManager.selectedUnit == null
        //   && SelectionManager.selectedTile != null
        //   && GUIData.activeButton.name.Contains("Move"))
        //{
        //    PathMovementManager.move(spawn.currentUnit, GUIData.targetTile);
        //    GUIData.activeButton = null;
        //    GUIData.canSelectTarget = false;
        //}
            if (Input.GetMouseButtonDown(1) && !GUIData.pointerOnGUI && GUIData.canSelectTarget && Data.isEndTurnPossible())
        {
            GUIData.activeButton = null;
            GUIData.canSelectTarget = false;
        }
*/
    }
}