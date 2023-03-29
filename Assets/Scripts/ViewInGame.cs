using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewInGame : MonoBehaviour
{
    public TextMeshProUGUI collectableLabel, scoreLabel, maxScoreLabel;
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.INGAME){
            int currentObjects = GameManager.sharedInstance.collectedObjects;
            this.collectableLabel.text = currentObjects.ToString();
        }
    }
}
