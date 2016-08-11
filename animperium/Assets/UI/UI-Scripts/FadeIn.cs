using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

    private CanvasRenderer[] renders;
    public float fadeSpeed = 3.91f;
    // Use this for initialization
    void Start () {
        renders = gameObject.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(Fade());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Fade() {
        foreach (CanvasRenderer c in renders) {
            c.SetAlpha(0f);
        }
        yield return 0;

        while (renders[0].GetAlpha() < 1f) {
            foreach (CanvasRenderer c in renders) {
                c.SetAlpha(c.GetAlpha() + Time.deltaTime * fadeSpeed);
            }
            yield return 0;
        }

        yield break;
    }
}
