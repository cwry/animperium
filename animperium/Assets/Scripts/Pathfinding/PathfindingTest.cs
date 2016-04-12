using UnityEngine;
using System.Collections;

public class PathfindingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vec2i[] path = PathFinding.findPath(Data.mainGrid, 0, 0, 5, 5);
        foreach (Vec2i v in path)
        {
            Debug.Log(v.x + " | " + v.y);
        }
    }
}
