using UnityEngine;
using System.Collections;

public class HealthBar: MonoBehaviour
{
    public GameObject foreGround;
    public GameObject unitObject;
    private Unit unit;
    Vector3 localScale;

    void Start()
    {
        localScale = foreGround.transform.localScale;
        unit = unitObject.GetComponent<Unit>();
    }

    void Update()
    {
        foreGround.transform.localScale = new Vector3((unit.hitPoints / unit.maxHitPoints)*localScale.x,localScale.y,localScale.z);
    }

}