using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;
    private Rigidbody2D rigidBody;
    public static bool turnAround;

    private Vector3 startPosition;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    private void Start() {
        this.transform.position = startPosition;
    }

    private void FixedUpdate() {

        float currentRunningSpeed = runningSpeed;

        //Gira el enemigo sobre su propio eje
        if(turnAround){
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180.0f, 0);
        }else{
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (GameManager.sharedInstance.currentGameState == GameState.INGAME)
        {
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        }
    }
}
