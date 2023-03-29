using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController sharedInstance;
    
    private Rigidbody2D rigidBody;

    private Vector3 startPosition;

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float runningSpeed = 5f;

    //Variable para detectar la capa del suelo
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        sharedInstance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        //Guardo siempre la pos inicial
        startPosition = this.transform.position;
    }

    public void StartGame()
    {
        animator.SetBool("isAlive", true);
        animator.SetBool("isGrounded", true);
        this.transform.position = startPosition;
    }

    private void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.INGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }

        //Lo hace aca porque el update se ejecuta cada frame, entonces todo el tiempo chequea si esta o no tocando el piso
        animator.SetBool("isGrounded", IsTouchingTheGround());
    }

    //El movimiento NO va en el update porque si hay una bajada de frames, el personaje va a hacer saltos
    //Fixed update sirve para ejecutarse a intervalos fijos, entonces no hace saltos dependiendo los fps
    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.INGAME)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity =
                    new Vector2(runningSpeed, rigidBody.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsTouchingTheGround()
    {
        //Trazar un rayo (raycast) desde la pos del personaje, mirando hacia abajo, a una distancia de 0.2 (20 centimetros), choco contra el suelo
        if (
            Physics2D
                .Raycast(this.transform.position,
                Vector2.down,
                0.2f,
                groundLayer)
        )
        {
            return true;
        }

        return false;
    }

    public void Kill(){
        GameManager.sharedInstance.GameOver();
        this.animator.SetBool("isAlive", false);
    }
}
