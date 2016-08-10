using UnityEngine;
using System.Collections;

public class MapTileOffsetHack : MonoBehaviour {

    public Material falseMat;
    public Material trueMat;
    int offsetX = 4;
    int offsetY = 4;

	// Use this for initialization
	void Awake () {
        MapGridContainer c = gameObject.GetComponent<MapGridContainer>();
        TileInfo[] tiles = c.GetComponentsInChildren<TileInfo>();
        Debug.Log(tiles.Length);
        bool[,] traversable = new bool[c.width, c.height];
        foreach (TileInfo t in tiles) {
            if(t.initGridX < offsetX || t.initGridY < offsetY) {
                Debug.Log(t.initGridX + " | " + t.initGridY + " got pushed out");
                continue;
            }
            traversable[t.initGridX - offsetX, t.initGridY - offsetY] = t.traversable;
            /*if(t.initGridX < c.width - offsetX || t.initGridY < c.height - offsetY) {
                Debug.Log(t.initGridX + " | " + t.initGridY + " got copied to " + (t.initGridX - offsetX) + " | " + (t.initGridY - offsetY) + " : " + t.traversable);
                traversable[t.initGridX - offsetX, t.initGridY - offsetY] = t.traversable;
            }
            Debug.Log(t.initGridX + " | " + t.initGridY + " got filled");*/
        }

        foreach(TileInfo t in tiles) {
            t.traversable = traversable[t.initGridX, t.initGridY];
            t.GetComponent<Renderer>().material = t.traversable ? trueMat : falseMat;
        }
    }
}
