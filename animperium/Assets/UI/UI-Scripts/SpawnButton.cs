using UnityEngine;
using System.Collections;

public class SpawnButton : MonoBehaviour {

	public void OnClick()
    {
        if(Data.isEndTurnPossible())
        {
            Vec2i spawn;
            if(Data.playerID == 1)
            {
                spawn = new Vec2i(22, 55);
            }
            else
            {
                spawn = new Vec2i(24, 54);
            }

            GameObject tile = Data.mainGrid.gridData[spawn.x, spawn.y];
            TileInfo tileInfo = tile.GetComponent<TileInfo>();
            if (tileInfo.unit != null) return;
            SpawnManager.spawnUnit(Data.mainGrid, spawn, "Worker");
            
        }
    }
}
