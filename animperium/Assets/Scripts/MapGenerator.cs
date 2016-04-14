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
        GridManager grid = isMain ? Data.mainGrid : Data.subGrid;
        //temp
        for(var y = 0; y < grid.gridHeightInHexes; y++){
            for(var x = 0; x < grid.gridWidthInHexes; x++){
                GameObject tile = grid.gridData[x, y];
                int perlin = mapPerlin(samplePerlin(tile.transform.position));
                tile.GetComponent<TileInfo>().traversable = perlin == 1;

                Color clr = Color.black;

                switch (perlin){
                    case 0:
                        clr = Color.blue;
                        break;
                    case 1:
                        clr = Color.yellow;
                        break;
                    case 2:
                        clr = Color.grey;
                        break;
                }
                tile.GetComponent<Renderer>().material.color = clr;
            }
        }
        //
    }

}
