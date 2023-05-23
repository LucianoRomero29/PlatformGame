using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;

public class PlayerController : MonoBehaviour
{
    public static PlayerController sharedInstance;
    
    private Rigidbody2D rigidBody;

    private Vector3 startPosition;

    //TODO: Esta variable es para el analytics para contar el total de muertes
    //Tengo una duda con esta , porque siempre va a volver a 0, como persisto ese dato?
    private int playersDead;

    [SerializeField]
    private float jumpForce = 50f;

    [SerializeField]
    private float runningSpeed = 7f;

    //Variable para detectar la capa del suelo
    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Animator animator;

    [SerializeField] private int healthPoints, energyPoints;

    private int doubleJump = 0;

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    public const int INITIAL_HEALTH = 100, INITIAL_ENERGY = 10, MAX_HEALTH = 150, MAX_ENERGY = 25;
    public const int MIN_HEALTH = 20;
    public const float MIN_SPEED = 2.5f, HEALTH_TIME_DECREASE = 1.0f;
    public const int SUPERJUMP_COST = 3;
    public const float SUPERJUMP_FORCE = 1.5f;

    private void Awake()
    {
        sharedInstance = this;
        rigidBody = GetComponent<Rigidbody2D>();
        
        //Guardo siempre la pos inicial
        startPosition = this.transform.position;

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
    }

    public void StartGame()
    {
        animator.SetBool("isAlive", true);
        animator.SetBool("isGrounded", true);

        this.transform.position = startPosition;
        this.healthPoints = INITIAL_HEALTH;
        this.energyPoints = INITIAL_ENERGY;

        // No me interesa cansar al jugador 
        // StartCoroutine(TirePlayer());
    }

    private void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.INGAME)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Jump(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Jump(true);
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
            //Acá antes usaba directamente la running speed, esto esta por si quiero ir haciendolo cada vez mas lento conforme pase el tiempo
            float currentSpeed = (runningSpeed - MIN_SPEED) * this.healthPoints / 100.0f;
            if (rigidBody.velocity.x < currentSpeed)
            {
                rigidBody.velocity =
                    new Vector2(currentSpeed, rigidBody.velocity.y);
            }
        }
    }

    private void Jump(bool isSuperJump)
    {
        if (IsTouchingTheGround())
        {
            if(isSuperJump && this.energyPoints >= SUPERJUMP_COST){
                energyPoints -= SUPERJUMP_COST;
                rigidBody.AddForce(Vector2.up * jumpForce * SUPERJUMP_FORCE, ForceMode2D.Impulse);
            }else{
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            doubleJump++;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"double_jump", doubleJump}
            };
            AnalyticsService.Instance.CustomData("doubleJump", parameters);

            audioSource.PlayOneShot(this.jumpSound);
        }
    }

    private bool IsTouchingTheGround()
    {   
        //Trazar un rayo (raycast) desde la pos del personaje, mirando hacia abajo, a una distancia de 0.2 (20 centimetros), choco contra el suelo
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 2.1f, groundLayer))
        {
            return true;
        }

        return false;
    }

    //Cansa al jugador cada 1seg
    IEnumerator TirePlayer(){
        while(this.healthPoints > MIN_HEALTH){
            this.healthPoints--;
            yield return new WaitForSeconds(HEALTH_TIME_DECREASE);
        }

        yield return null;
    }

    public void Kill(){
        GameManager.sharedInstance.GameOver();
        this.animator.SetBool("isAlive", false);

        int currentMaxScore = PlayerPrefs.GetInt("maxScore", 0);
        if(currentMaxScore < this.GetDistance()){
            PlayerPrefs.SetInt("maxScore", this.GetDistance());
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"level_index", GameManager.sharedInstance.levelIndex},
            {"total_score", GameManager.sharedInstance.collectedObjects},
            {"total_distance", this.GetDistance()},
            {"players_dead", playersDead++},
        };

        AnalyticsService.Instance.CustomData("gameOver", parameters);
    }

    public int GetDistance(){
        //Ignoro la Y por eso 0f en la Y
        int traveledDistance = (int) Vector2.Distance(new Vector2(startPosition.x, 0f), new Vector2(this.transform.position.x, 0f));

        return traveledDistance;
    }

    public void CollectHealth(int points){
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH){
            this.healthPoints = MAX_HEALTH;
        }
    }

    public void CollectEnergy(int points){
        this.energyPoints += points;
         if(this.energyPoints >= MAX_ENERGY){
            this.energyPoints = MAX_ENERGY;
        }
    }

    public int GetHealth(){
        return this.healthPoints;
    }   

    public int GetEnergy(){
        return this.energyPoints;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.tag == "Enemy"){
            this.healthPoints -= 15;
        }

        if(otherCollider.tag == "Rock"){
            this.healthPoints -= 5;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.INGAME && this.healthPoints <= 0){
            Kill();
        }
    }
}
