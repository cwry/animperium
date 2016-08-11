using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    private CanvasRenderer[] renders;
    public float fadeSpeed = 1f;
    public float stayTime = 3f;
    public float waitUntilStart = 0f;
    // Use this for initialization
    void Start () {
        renders = gameObject.GetComponentsInChildren<CanvasRenderer>();
        foreach (CanvasRenderer c in renders) {
            c.SetAlpha(0f);
        }
        StartCoroutine(StartFade());
    }

    private IEnumerator StartFade() {
        yield return new WaitForSeconds(waitUntilStart);
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
        yield return new WaitForSeconds(stayTime);
        while (renders[0].GetAlpha() > 0f) {
            foreach (CanvasRenderer c in renders) {
                c.SetAlpha(c.GetAlpha() - Time.deltaTime * fadeSpeed);
            }
            yield return 0;
        }

        yield break;
    }
}
