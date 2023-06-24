using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewInGame : MonoBehaviour
{
    public TextMeshProUGUI collectableLabel, scoreLabel, maxScoreLabel;
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.INGAME ||
            GameManager.sharedInstance.currentGameState == GameState.GAMEOVER){
            int currentObjects = GameManager.sharedInstance.collectedObjects;
            this.collectableLabel.text = currentObjects.ToString();
        }

        if(GameManager.sharedInstance.currentGameState == GameState.INGAME){
            int traveledDistance = PlayerController.sharedInstance.GetDistance();
            this.scoreLabel.text = "Distancia\n"+traveledDistance.ToString("f0");

            int maxScore = PlayerPrefs.GetInt("maxScore", 0);
            this.maxScoreLabel.text = "MaxScore\n" + maxScore.ToString("f0");
        }
    }
}
