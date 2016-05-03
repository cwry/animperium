using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

    public Vector2 seed;
    public float scale;
    public bool isMain;
	
    public float samplePerlin(Vector3 pos){
        return Mathf.PerlinNoise(seed.x + pos.x * scale, seed.y + pos.z * scale);
    }

    public int mapPerlin(float perlin){
        if (perlin < 0.2) return 0;
        if (perlin < 0.6) return 1;
        return 2;
    }

    void Awake(){
        Random.seed = MapLoadData.seed;
        seed = new Vector2(Random.value * 99999, Random.value * 99999);
        GridManager grid = isMain ? Data.mainGrid : Data.subGrid;
        //temp
        for(var y = 0; y < grid.gridHeightInHexes; y++){
            for(var x = 0; x < grid.gridWidthInHexes; x++){
                GameObject tile = grid.gridData[x, y];
                int perlin = mapPerlin(samplePerlin(isMain ? tile.transform.position : tile.transform.position - Data.subGrid.offsetInit));
                TileInfo info = tile.GetComponent<TileInfo>();
                info.traversable = perlin == 1;

                Color clr = Color.black;

                switch (perlin){
                    case 0:
                        clr = isMain ? Color.blue : Color.black;
                        break;
                    case 1:
                        clr = isMain ? Color.yellow : Color.white;
                        break;
                    case 2:
                        clr = isMain ? Color.grey : Color.black;
                        break;
                }
                //tile.GetComponent<Renderer>().material.color = clr;
            }
        }
        //
    }

}
