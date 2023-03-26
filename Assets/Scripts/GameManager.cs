using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void StartGame(){
        SetGameState(GameState.INGAME);

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
        //Reseteo la camara cuando el pj muere porque sino se va a cualquier lado y hace un efecto feo
        cameraFollow.ResetCameraPosition();

        //Solo si se movio un poco (10 = 10 metros)
        if(PlayerController.sharedInstance.transform.position.x > 10){
            //Puede pasar que reinicio el juego y NO haya levelblock, por lo tanto remuevo todo y luego genero nuevos
            LevelGenerator.sharedInstance.RemoveAllTheBlocks();
            LevelGenerator.sharedInstance.GenerateInitialBlocks();
        }

        PlayerController.sharedInstance.StartGame();
    }

    public void GameOver(){
        SetGameState(GameState.GAMEOVER);
    }

    public void BackToMenu(){
        SetGameState(GameState.MENU);
    }

    //Metodo encargado de cambiar el estado del juego
    public void SetGameState(GameState newGameState){

        //Segun que estado sea el nuevo, tengo que llamar una escena nueva
        if(newGameState == GameState.MENU){

        }
        else if(newGameState == GameState.INGAME){
            
        }
        else if(newGameState == GameState.GAMEOVER){
            
        }

        this.currentGameState = newGameState;
    }
}
