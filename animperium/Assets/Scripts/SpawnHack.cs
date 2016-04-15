using UnityEngine;
using System.Collections;

public class SpawnHack : MonoBehaviour {

    public delegate bool ArchOrSword();

    public int unitCount = 30;
    public GameObject[] units;
    public GameObject swordFighter;
    public GameObject archer;
    public GameObject worker;


	// Use this for initialization
	void Start () {
        units = new GameObject[unitCount];
        for(int i = 0; i < units.Length; i++)
        {
            GameObject unit = units[i];
            float random = Random.Range(0f, 100f);
            if (random <= 50) unit = Instantiate(archer); else unit = Instantiate(swordFighter);
            GameObject testHex = RandomTile();
            while (!testHex.GetComponent<TileInfo>().traversable) testHex = RandomTile();
            if (testHex.GetComponent<TileInfo>().traversable)
            {
                
                unit.transform.position = testHex.transform.position;
                unit.GetComponent<Unit>().currentTile = testHex;
                testHex.GetComponent<TileInfo>().unit = unit;
            }
            
        }
	}
	
    private GameObject RandomTile()
    {
        GameObject[] hexs = GameObject.FindGameObjectsWithTag("Hex");
        int rndTile = (int)Random.Range(0f, (float)hexs.Length);
        return hexs[rndTile];
    }
	// Update is called once per frame
	void Update () {
	
	}
}
