using UnityEngine;
using System.Collections;

public class PathfindingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        /*bool[,] collisionGrid = new bool[Data.mainGrid.gridHeightInHexes, Data.mainGrid.gridWidthInHexes];
        for(int y = 0; y < Data.mainGrid.gridHeightInHexes; y++){
            for(int x = 0; x < Data.mainGrid.gridWidthInHexes; x++){
                GameObject tile = Data.mainGrid.gridData[x, y];
                bool perlin = Mathf.PerlinNoise(100 + tile.transform.position.x * 0.08f, 100 + tile.transform.position.z * 0.08f) > 0.47;
                float clr = perlin ? 1 : 0;
                tile.GetComponent<Renderer>().material.color = new Color(clr, clr, clr, 1);
                collisionGrid[x, y] = perlin;
            }
        }*/


        Vec2i[] path = PathFinding.findPath(Data.mainGrid, 42, 30, 10, 10, 
            (Vec2i hex) => {
                return Data.mainGrid.gridData[hex.x, hex.y].GetComponent<TileInfo>().traversable;
            });

        PathMovement.move(gameObject, Data.mainGrid, path, 3f, (GameObject go) => { Debug.Log(gameObject.name + " is done following path"); });


        GameEvent onSuperAwesome = new GameEvent();


        onSuperAwesome.add<string>((string str) => { Debug.Log(str); });

        onSuperAwesome.fire("super awesome event");



        if (path == null) return;
        foreach (Vec2i v in path){
            //Debug.Log(v.x + " | " + v.y);
            Data.mainGrid.gridData[v.x, v.y].GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
