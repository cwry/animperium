using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnBarBlinking : MonoBehaviour {

   
    private Image turnbarImage;
    public Sprite zero;
    public Sprite normal;
    public Sprite blue;
    public Sprite red;
    public Sprite green;

    private int turn;

   

    public 
    // Use this for initialization
	void Start () {
        turn = TurnManager.turnID;
        turnbarImage = gameObject.GetComponent<Image>();
        turnbarImage.sprite = normal;
        StartCoroutine(Highlight(2f, 1f, blue));


    }

    public void RestartBlinking()
    {
        StopAllCoroutines();
        Start();
    }
    

    public IEnumerator Highlight(float timeFrequence, float timeDisplayed, Sprite highlightSprite )
    {
        Sprite savedOld;
        while(gameObject)
        {
            yield return new WaitForSeconds(timeFrequence);
            savedOld = turnbarImage.sprite;
            turnbarImage.sprite = highlightSprite;
            yield return new WaitForSeconds(timeDisplayed);
            turnbarImage.sprite = savedOld;
        }
        yield return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnManager.turnID != turn)
        {
            RestartBlinking();
        }
       
            
    }
}
