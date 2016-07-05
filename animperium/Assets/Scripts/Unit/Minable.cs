using UnityEngine;
using System.Collections;

public enum Resource{
    WOOD, STONE, IRON, GOLD
}

public class Minable : MonoBehaviour {
    public float amt;
    public Resource type;
}
