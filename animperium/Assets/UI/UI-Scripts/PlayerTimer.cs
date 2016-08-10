using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTimer : MonoBehaviour {

    public Text playerTimerText;
    public Text goldText;

    private float playerTime = 0;
    private int turn;
    public int maxRoundTime = 180;
    private int turnGold = 0;
    public int maxGold = 100;
    private bool isActive;

    // Use this for initialization
    void Awake () {
        playerTimerText.text = "";
        goldText.text = "";
        turn = TurnManager.turnID;
        TurnManager.onTurnBegin.add<int>((int turnID) => {
            if (Data.isActivePlayer()) {
                isActive = true;
            }
        });
        TurnManager.onTurnEnd.add<int>((int turnID) => {
            if (Data.isActivePlayer()) {
                isActive = false;
                playerTime = 0;
                GUIData.roundTime = (int)playerTime;
                playerTimerText.text = "";
                goldText.text = "";
                Data.gold += turnGold;
            }
        });
    }
	
	// Update is called once per frame
	void Update () {

        if (playerTime >= maxRoundTime && Data.isActivePlayer()) {
            if(!GUIData.blockAction) TurnManager.endTurn();
        }
        if (isActive) {
            playerTime = playerTime + Time.deltaTime;
            int time =(int) (maxRoundTime - playerTime);
            playerTimerText.text = PlayerTimeToString(time);
            turnGold = PlayerTimeToGold((int)playerTime);
            goldText.text = "+" + turnGold.ToString();
            GUIData.roundTime = (int)playerTime;
        }
	}
    
    private string PlayerTimeToString(int seconds)
    {
        int min = seconds / 60;
        int sec = seconds % 60;
        return (min.ToString() + ":" + sec.ToString("d2"));
    }
   
    private int PlayerTimeToGold(int time) {
        float remainingTime = maxRoundTime - playerTime;
        int gold = (int)(maxGold * remainingTime / maxRoundTime);
        return gold;
    }
}
