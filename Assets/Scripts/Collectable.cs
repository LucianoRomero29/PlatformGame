using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType{
    healthPotion,
    energyPotion,
    money
}

[RequireComponent(typeof(AudioSource))]
public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    private bool isCollected = false;
    private int flagCollected = 0;

    private AudioSource audioSource;

    [SerializeField] private int value = 0;
    [SerializeField] private AudioClip collectSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
    }
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
     
        // if(audioSource != null && this.collectSound != null){
        //     audioSource.PlayOneShot(this.collectSound);
        // }

        switch(this.type){
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(value);
                flagCollected = 0;
                break;
            case CollectableType.healthPotion:
                PlayerController.sharedInstance.CollectHealth(value);
                break;
            case CollectableType.energyPotion:
                PlayerController.sharedInstance.CollectEnergy(value);
                break;
        }
       
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
