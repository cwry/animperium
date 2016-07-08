using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour {

    public ParticleSystem ps;

	void Update () {
        if (ps) {
            if (!ps.IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}
