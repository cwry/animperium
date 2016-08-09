using UnityEngine;
using System.Collections;

public class UndergroundManager : MonoBehaviour {

    public GameObject hiddenPrefab;
    public GameObject blockedPrefab;
    public GameObject revealedPrefab;
    public GameObject datedBlockedPrefab;
    public GameObject datedRevealedPrefab;

    private static UndergroundManager instance;

    void Awake() {
        instance = this;
    }

    public static GameObject getAppearancePrefab(UndergroundTileAppearanceState appearance, bool isInSight) {
        switch (appearance) {
            case UndergroundTileAppearanceState.HIDDEN :
                return instance.hiddenPrefab;
            case UndergroundTileAppearanceState.BLOCKED :
                return isInSight ? instance.blockedPrefab : instance.datedBlockedPrefab;
            case UndergroundTileAppearanceState.REVEALED :
                return isInSight ? instance.revealedPrefab : instance.datedRevealedPrefab;
        }
        return null;
    }
}
