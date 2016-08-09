using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTimer : MonoBehaviour {

    public Text playerTimerText;
    public Text goldText;

    private int playerTime;
    private int turn;
    public int maxRoundTime = 180;
    private int turnGold = 0;
    public int maxGold = 100;
    private bool isActive;

    // Use this for initialization
    void Awake () {
        turn = TurnManager.turnID;
        StartCoroutine(TimerStart());
        TurnManager.onTurnBegin.add<int>((int turnID) => {
            if (Data.isActivePlayer()) {
                StartCoroutine(TimerStart());
                isActive = true;
            }
        });
        TurnManager.onTurnEnd.add<int>((int turnID) => {
            if (Data.isActivePlayer()) {
                StopAllCoroutines();
                isActive = false;
                playerTime = 0;
                playerTimerText.text = "";
                goldText.text = "";
                Data.gold += turnGold;
            }
        });
    }
	
	// Update is called once per frame
	void Update () {

        if (playerTime >= maxRoundTime && Data.isActivePlayer()) {
            TurnManager.endTurn();
        }
        if (isActive) {
            int time = maxRoundTime - playerTime;
            playerTimerText.text = PlayerTimeToString(time);
            turnGold = PlayerTimeToGold(playerTime);
            goldText.text = turnGold.ToString(); ;
            GUIData.roundTime = playerTime;
        }
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
        return (min.ToString() + ":" + sec.ToString("d2"));
    }
   
    private int PlayerTimeToGold(int time) {
        int remainingTime = maxRoundTime - playerTime;
        int gold = (int)(maxGold * remainingTime / (float)maxRoundTime);
        return gold;
    }
}
