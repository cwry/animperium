using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour {

    private CanvasRenderer[] renders;
    public float fadeSpeed = 1f;
    public float stayTime = 3f;

	// Use this for initialization
	void Start () {
        renders = gameObject.GetComponentsInChildren<CanvasRenderer>();
        foreach (CanvasRenderer c in renders) {
            c.SetAlpha(0f);
        }
    }
	
    public void StartFade() {
        StartCoroutine(FadeInOut());
    }
    private IEnumerator FadeIn()
    {
        foreach(CanvasRenderer c in renders)
        {
            c.SetAlpha(0f);
        }
        yield return 0;

        while(renders[0].GetAlpha() < 1f)
        {
                foreach (CanvasRenderer c in renders)
                {
                c.SetAlpha(c.GetAlpha() + Time.deltaTime * fadeSpeed);
                }
            yield return 0;
        }

        yield break;
    }

    private IEnumerator FadeOut() {
        while (renders[0].GetAlpha() > 0f) {
            foreach (CanvasRenderer c in renders) {
                c.SetAlpha(c.GetAlpha() - Time.deltaTime * fadeSpeed);
            }
            yield return 0;
        }

        yield break;
    }

    private IEnumerator FadeInOut() {
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
