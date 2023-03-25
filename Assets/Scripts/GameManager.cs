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
        if(Input.GetKeyDown(KeyCode.S)){
            StartGame();
        }
    }

    public void StartGame(){
        SetGameState(GameState.INGAME);
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
