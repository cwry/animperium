using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTimer : MonoBehaviour {

    public Text player1TimerText;
    public Text player2TimerText;
    private Text actualText;

    private int playerTime;
    private Coroutine timer;
    private int turn;


    // Use this for initialization
    void Start () {
        turn = TurnManager.turnID;
        actualText = player1TimerText;
        timer = StartCoroutine(TimerStart());
	}
	
	// Update is called once per frame
	void Update () {

        if (playerTime == 900)
        {
            TurnManager.endTurn();
        }

        if (turn != TurnManager.turnID)
        {
            turn = TurnManager.turnID;
            StopAllCoroutines();
            playerTime = 0;
            actualText.text = PlayerTimeToString(playerTime);

            if (actualText == player1TimerText)
            {
                actualText = player2TimerText;
            }
            else
            {
                actualText = player1TimerText;
            }

            StartCoroutine(TimerStart());
        }
        
        actualText.text = PlayerTimeToString(playerTime);
        GUIData.roundTime = playerTime;
       
	}

   private IEnumerator TimerStart()
    {
        for (int i = 0; i <= 900; i++ )
        {
            playerTime = i;
            yield return new WaitForSeconds(1f);
        }
        yield return 0;
    }

    private string PlayerTimeToString(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        return (min.ToString("d2") + ":" + sec.ToString("d2"));
    }
   
}
