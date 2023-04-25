using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    public bool movingForward;

    private void OnTriggerEnter2D(Collider2D otherCollider) {

        if(otherCollider.tag == "Collectable") {
            return;
        }

        if(movingForward){
            Enemy.turnAround = true;
        }else{
            Enemy.turnAround = false;
        }

        movingForward = !movingForward;
    }
}
