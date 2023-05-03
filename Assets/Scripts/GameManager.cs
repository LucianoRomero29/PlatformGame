using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

//Estados del videojuego para manejarlos a traves del manager
public enum GameState{
    MENU,
    INGAME,
    GAMEOVER
}

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance;
    public GameState currentGameState = GameState.MENU;
    [SerializeField] private Canvas menuCanvas, gameCanvas, gameOverCanvas;
    public int collectedObjects = 0;
    public int levelIndex = 1;
    private int playersGame;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        BackToMenu();
    }

    private void Update() {
        if(Input.GetButtonDown("Start") && currentGameState != GameState.INGAME){
            StartGame();
        }

        if(Input.GetButtonDown("Pause")){
            BackToMenu();
        }

        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Escape)){
            ExitGame();
        }
        #endif
    }

    public void StartGame(){

        //TODO: Evento de level start
        // Analytics.CustomEvent("level_start", new Dictionary<string, object>{
        //     {"players_game", playersGame}
        // });

        SetGameState(GameState.INGAME);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
        //Reseteo la camara cuando el pj muere porque sino se va a cualquier lado y hace un efecto feo
        cameraFollow.ResetCameraPosition();

        //Solo si se movio un poco (10 = 10 metros)
        if(PlayerController.sharedInstance.transform.position.x > 10){
            //Puede pasar que reinicio el juego y NO haya levelblock, por lo tanto remuevo todo y luego genero nuevos
            LevelGenerator.sharedInstance.RemoveAllTheBlocks();
            LevelGenerator.sharedInstance.GenerateInitialBlocks(true);
        }

        PlayerController.sharedInstance.StartGame();

        collectedObjects = 0;
    }

    public void GameOver(){
        SetGameState(GameState.GAMEOVER);
    }

    public void BackToMenu(){
        SetGameState(GameState.MENU);
    }

    //Para mobile no es necesario esto. 
    public void ExitGame(){
        //Application.Quit();
        //Se utilizan estos IF / ELSE con # indicando que es una decision basada en la plataforma que se va a ejecutar
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //Metodo encargado de cambiar el estado del juego
    public void SetGameState(GameState newGameState){
        //Segun que estado sea el nuevo, tengo que llamar una escena nueva
        if(newGameState == GameState.MENU){
            menuCanvas.enabled = true;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = false;
        }
        else if(newGameState == GameState.INGAME){
            menuCanvas.enabled = false;
            gameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
        }
        else if(newGameState == GameState.GAMEOVER){
            menuCanvas.enabled = false;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = true;
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(int objectValue){
        this.collectedObjects += objectValue;
    }
}
