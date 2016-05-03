using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTimer : MonoBehaviour {

    public Text player1TimerText;
    public Text player2TimerText;
    private Text actualText;

    private int playerTime;
    private Coroutine timer;
    private IEnumerator timerValue;


    // Use this for initialization
    void Start () {
        actualText = player1TimerText;
        timerValue = TimerStart();
        timer = StartCoroutine(timerValue);
	}
	
	// Update is called once per frame
	void Update () {
        
        actualText.text = SecondToPlayerTime(playerTime);

	}

   private IEnumerator TimerStart()
    {
        for (int i = 0; i <= 900; i++ )
        {
            yield return new WaitForSeconds(1f);
            playerTime = i;
            


        }
        yield return 0;
    }

    private string SecondToPlayerTime(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        return  (min + ":" + sec);
    }
   
}
