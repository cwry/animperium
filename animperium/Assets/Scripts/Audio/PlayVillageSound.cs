using UnityEngine;
using System.Collections;

public class PlayVillageSound : MonoBehaviour {

    
    public int circleSize = 20;
    public int minUnitNumber = 1;

    private GameObject[] environment;
    //
    private float loopFrequenz = 1f;
    private float requestTime;
    private SoundManager soundManager;

    void Awake() {
        environment = AoeChecks.getCircle(circleSize, 3)(gameObject.GetComponent<Unit>().currentTile.GetComponent<TileInfo>());
        requestTime = Time.time + loopFrequenz;
        soundManager = SoundManager.instance;
    }
	
    void Update() {
        if (Time.time >= requestTime) {
            if (gameObject.GetComponent<Unit>().playerID == 1 && isUnderAttack(countEnemys())) {
                if (!soundManager.isPlaying("villagedefense")) {
                    soundManager.SetVolume("ingameloop",0f);
                    soundManager.PlaySound("villagedefense", SoundManager.musicVolume
                        , () => {
                            soundManager.SetVolume("ingameloop", 1f);
                            Update();
                        });
                }
            }
            else if (gameObject.GetComponent<Unit>().playerID == 2 && isUnderAttack(countMyAttackers())) {
                if (!soundManager.isPlaying("villageattack")) {
                    soundManager.SetVolume("ingameloop",0f);
                    soundManager.PlaySound("villageattack", SoundManager.musicVolume
                        ,() => {
                            soundManager.SetVolume("ingameloop", SoundManager.musicVolume);
                            Update();
                        });
                }
            }
            requestTime = Time.time + loopFrequenz;
        }
    }
    private bool isUnderAttack(int unitNumber) {
        if (unitNumber >= minUnitNumber) return true;
        else return false;
        
    }

    private int countEnemys() {
        int enemyCount = 0;
        foreach(GameObject g in environment) {
            GameObject unit = g.GetComponent<TileInfo>().unit;
            if (unit != null && unit.GetComponent<Unit>().playerID == 2) {
                enemyCount++;
            }
        }
        return enemyCount;
    }

    private int countMyAttackers() {
        int attackers = 0;
        foreach (GameObject g in environment) {
            GameObject unit = g.GetComponent<TileInfo>().unit;
            if (unit != null && unit.GetComponent<Unit>().playerID == 1) {
                attackers++;
            }
        }
        return attackers;
    }
}
