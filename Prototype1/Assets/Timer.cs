using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{	
	public Text timerText;
	public float gameTimer;

    public GameM manager;

    // Update is called once per frame
    void Update()
    {
        if(manager.getGameWin() == false && manager.getGameEnded() == false) {
            gameTimer -= Time.deltaTime;
        }

        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer / 60) % 60;

        string timerString = string.Format("{0:0}:{1:00}", minutes, seconds);
    
        timerText.text = timerString;
    }
}
