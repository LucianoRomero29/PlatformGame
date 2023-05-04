using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

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
    [SerializeField] private Canvas menuCanvas, gameCanvas, pauseCanvas, gameOverCanvas;
    public int collectedObjects = 0;
    public int levelIndex = 1;
    private int playersGame = 0, pauseGame = 0, restartLevel;

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

        if(Input.GetKeyDown(KeyCode.P)){
            PauseGame();
        }
    }

    public void StartGame(){
        playersGame++;
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"players_game", playersGame}
        };
        AnalyticsService.Instance.CustomData("levelStart", parameters);

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

    public void PauseGame(){
        //Esto estaba configurado en el manager con una tecla, sacarlo, ahora voy a llamar a esta funcion con un boton
        if(!menuCanvas.enabled){
            pauseCanvas.enabled = true;
            Time.timeScale = 0;

            pauseGame++;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"pause_game", pauseGame}
            };
            AnalyticsService.Instance.CustomData("pauseGame", parameters);
        }
    }

    public void ResumeGame(){
        pauseCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void RestartGame(){
        Time.timeScale = 1;
        restartLevel++;
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"restart_level", restartLevel}
        };

        AnalyticsService.Instance.CustomData("restartLevel", parameters);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu(){
        SetGameState(GameState.MENU);
    }

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
            pauseCanvas.enabled = false;
        }
        else if(newGameState == GameState.INGAME){
            menuCanvas.enabled = false;
            gameCanvas.enabled = true;
            gameOverCanvas.enabled = false;
            pauseCanvas.enabled = false;
        }
        else if(newGameState == GameState.GAMEOVER){
            menuCanvas.enabled = false;
            gameCanvas.enabled = false;
            gameOverCanvas.enabled = true;
            pauseCanvas.enabled = false;
        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(int objectValue){
        this.collectedObjects += objectValue;
    }
}
