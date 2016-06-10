using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapEditorHelper : MonoBehaviour {
    public GameObject editorGrid;
    public Material trueMat;
    public Material falseMat;

    void Update(){
        TileInfo[] tiles = editorGrid.GetComponentsInChildren<TileInfo>();
        foreach(TileInfo tile in tiles){
            tile.gameObject.GetComponent<Renderer>().material = tile.traversable ? trueMat : falseMat;
        }
    }
}
