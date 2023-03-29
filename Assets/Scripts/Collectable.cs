using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private bool isCollected = false;
    private int flagCollected = 0;

    [SerializeField] private int value = 0;

    private void Show(){
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        isCollected = false;
    }

    private void Hide(){
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void Collect(){
        isCollected = true;
        Hide();
        GameManager.sharedInstance.CollectObject(value);
        flagCollected = 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            if(flagCollected == 0){
                flagCollected++;
                Collect();
            }
            
        }
    }
}
