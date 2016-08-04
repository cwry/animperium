using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetingManager : MonoBehaviour {

    static TargetingManager instance;

    bool isActive = false;
    public GameObject markerPrefab;
    public GameObject cursorPrefab;
    public GameObject rangePrefab;
    List<GameObject> markers = new List<GameObject>();
    List<GameObject> cursors = new List<GameObject>();
    List<GameObject> ranges = new List<GameObject>();
    GameObject[] currentTargets;
    GameObject[] currentRanges;
    Action<GameObject> currentCallback;
    Action currentCancelledCallback;
    GameObject lastCursorTarget = null;
    Func<TileInfo, GameObject[]> getCursor;

    void Awake(){
        instance = this;
    }

    void Update(){
        if (!isActive) return;
        if (Input.GetMouseButtonDown(0)){
            GameObject target = null;
            foreach (GameObject go in currentTargets){
                if (go == SelectionManager.hoverTile) target = go;
            }
            if (target != null) {
                currentCallback(target);
                deactivate();
            }
        }else if (Input.GetMouseButtonDown(1)){
            deactivate();
            currentCancelledCallback();
        }else if (lastCursorTarget != SelectionManager.hoverTile){
            foreach (GameObject go in currentTargets){
                if (go == SelectionManager.hoverTile) lastCursorTarget = go;
            }
            if(lastCursorTarget == SelectionManager.hoverTile){
                redrawCursor(lastCursorTarget);
            }else{
                deactivateCursors();
                lastCursorTarget = SelectionManager.hoverTile;
            }
        }
    }

    void redrawCursor(GameObject tile){
        GameObject[] currCursor = getCursor(tile.GetComponent<TileInfo>());
        for (int i = 0; i < currCursor.Length || i < cursors.Count; i++){
            if (i >= currCursor.Length){
                cursors[i].SetActive(false);
                continue;
            }

            if (i >= cursors.Count){
                GameObject go = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(transform);
                cursors.Add(go);
            }

            cursors[i].SetActive(true);
            cursors[i].transform.position = currCursor[i].transform.position;
        }
    }

    void redraw(){
        for (int i = 0; i < currentTargets.Length || i < markers.Count; i++){
            if (i >= currentTargets.Length){
                markers[i].SetActive(false);
                continue;
            }

            if (i >= markers.Count){
                GameObject go = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(transform);
                markers.Add(go);
            }
            markers[i].SetActive(true);
            markers[i].transform.position = currentTargets[i].transform.position;
        }

        for(int i = 0; i < currentRanges.Length || i < ranges.Count; i++) {
            if (i >= currentRanges.Length) {
                ranges[i].SetActive(false);
                continue;
            }

            if (i >= ranges.Count) {
                GameObject go = Instantiate(rangePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(transform);
                ranges.Add(go);
            }
            ranges[i].SetActive(true);
            ranges[i].transform.position = currentRanges[i].transform.position;
        }
    }

    void deactivate(){
        isActive = false;
        deactivateCursors();
        deactivateMarkers();
        deactivateRanges();
    }

    void deactivateMarkers(){
        foreach (GameObject go in markers){
            go.SetActive(false);
        }
    }

    void deactivateRanges() {
        foreach (GameObject go in ranges) {
            go.SetActive(false);
        }
    }

    void deactivateCursors(){
        foreach (GameObject go in cursors){
            go.SetActive(false);
        }
    }

    public static bool getActive(){ 
        return instance.isActive;
    }

    public static void selectTarget(GameObject[] targets, GameObject[] ranges, Action<GameObject> callback, Action cancelledCallback, Func<TileInfo, GameObject[]> getCursor = null){
        instance.isActive = true;
        if (getCursor == null) getCursor = AoeChecks.dot;
        instance.getCursor = getCursor;
        instance.currentTargets = targets;
        if (targets == null) instance.currentTargets = new GameObject[0];
        instance.currentRanges = ranges;
        if (ranges == null) instance.currentRanges = new GameObject[0];
        instance.currentCallback = callback;
        instance.currentCancelledCallback = cancelledCallback;
        instance.redraw();
    }

}
