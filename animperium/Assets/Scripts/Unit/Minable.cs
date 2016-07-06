using UnityEngine;
using System.Collections;

public enum Resource{
    WOOD, STONE, IRON, GOLD
}

public class Minable : MonoBehaviour {
    public float amt;
    public Resource type;

    public void mine(float mineAmount) {
        amt -= mineAmount;
        if (amt <= 0) mineAmount += amt;
        switch (type) {
            case Resource.IRON :
                Data.iron += mineAmount;
                break;
            case Resource.STONE :
                Data.stone += mineAmount;
                break;
            case Resource.WOOD:
                Data.wood += mineAmount;
                break;
        }
        Debug.Log("Mined " + mineAmount + " of type " + type);
        if (amt <= 0) Destroy(gameObject);
    }
}
