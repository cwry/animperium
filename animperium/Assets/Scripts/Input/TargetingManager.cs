using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetingManager : MonoBehaviour
{

    static TargetingManager instance;

    bool isActive = false;
    public GameObject markerPrefab;
    List<GameObject> markers = new List<GameObject>();
    GameObject[] currentTargets;
    Action<GameObject> currentCallback;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!isActive) return;
        if (Input.GetMouseButtonDown(0))
        {
            GameObject target = null;
            foreach (GameObject go in currentTargets)
            {
                if (go == SelectionManager.hoverTile) target = go;
            }
            if (target == null) return;
            currentCallback(target);
            deactivate();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            deactivate();
        }
    }

    void redraw()
    {
        for (int i = 0; i < currentTargets.Length || i < markers.Count; i++)
        {
            if (i >= currentTargets.Length)
            {
                markers[i].SetActive(false);
                continue;
            }

            if (i >= markers.Count)
            {
                GameObject go = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(transform);
                markers.Add(go);
            }
            markers[i].SetActive(true);
            markers[i].transform.position = currentTargets[i].transform.position;
        }
    }

    void deactivate()
    {
        isActive = false;
        foreach (GameObject go in markers)
        {
            go.SetActive(false);
        }
    }

    public static bool getActive()
    {
        return instance.isActive;
    }

    public static void selectTarget(GameObject[] targets, Action<GameObject> callback)
    {
        instance.isActive = true;
        instance.currentTargets = targets;
        instance.currentCallback = callback;
        instance.redraw();
    }

}
