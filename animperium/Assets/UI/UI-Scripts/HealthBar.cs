using UnityEngine;
using System.Collections;

public class HealthBar: MonoBehaviour
{
    public GameObject foreGround;
    public GameObject unitObject;
    private Unit unit;
    Vector3 localScale;

    public Material player1;
    public Material player2;

    void Start()
    {
        localScale = foreGround.transform.localScale;
        unit = unitObject.GetComponent<Unit>();
        if(unit.playerID == 1)
        {
            foreGround.GetComponent<Renderer>().material = player1;
        }
        else
        {
            foreGround.GetComponent<Renderer>().material = player2;
        }
    }

    void Update()
    {
        foreGround.transform.localScale = new Vector3((unit.hitPoints / unit.maxHitPoints)*localScale.x,localScale.y,localScale.z);
    }

}