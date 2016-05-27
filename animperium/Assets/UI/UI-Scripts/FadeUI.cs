using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour {

    private CanvasRenderer[] renders;
    public float fadeSpeed = 1f;

	// Use this for initialization
	void Start () {
        renders = gameObject.GetComponentsInChildren<CanvasRenderer>();
        StartCoroutine(FadeIn());
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
	// Update is called once per frame
	void Update () {
	
	}
}
