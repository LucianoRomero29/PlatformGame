using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject game, menu, credits;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource[] sfxSource;
    private bool muteMusic = false, muteSFX = false, changeLanguage = false;
    private int viewCredits, leaveGame, backMenu, restartLevel;
    public static UIManager sharedInstance;

    private void Awake() {
        sharedInstance = this;
    }

    //TODO: Acá va el evento pauseGame
    public void PauseGame(){
        //Esto estaba configurado en el manager con una tecla, sacarlo, ahora voy a llamar a esta funcion con un boton
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    //TODO: Esta funcion es parte del evento pauseGame es para saber cuántas personas se van del juego desde el menu
    public void LeaveGame(){
        //Esta es la variable a mandar
        leaveGame++;

        //Esta es otra variable a guardar, en que nivel lo abandonan
        int levelIndex = GameManager.sharedInstance.levelIndex;

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BackToMenu(){
        //Esta es la variable a mandar
        backMenu++;

        //Acá hay que hacer un restartLevel basicamente porque no quiero hacer 2 escenas
        //Si las hago tengo que hacer un loading screen por si hay demoras

        RestartLevel(true); //Envío un true para que NO sume en el restartLevel
    }

    //TODO: Acá ta el evento restartLevel
    public void RestartLevel(bool fromMenu = false){
        if(!fromMenu){
            //Si es falso, sumo la variable a mandar al analytics
            restartLevel++;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //TODO: Acá va el evento viewCredits
    public void OpenCredits(){
        credits.SetActive(true);
        //Esta es la variable a mandar
        viewCredits++;
    }


    //TODO: Acá tengo que agregar el evento mute
    //Creo que lo hicimos mal, enviamos un booleano al analytics y pusimos cuantas personas mutean el sonido
    //Ver bien después que enviar realmente
    public void MuteAndDesmuteSound(){
        audioSource.mute = !muteMusic;
        muteMusic = !muteMusic;
    }

    public void MuteAndDesmuteSFX(){
        //Acá voy a agregar en un array, todos los SFX que tenga y los voy a mutear uno x uno
        //Player, Colleccionables
        foreach (var sfx in sfxSource)
        {
            sfx.mute = !muteSFX;
        }

        muteSFX = !muteSFX;
    }

    //Acá va el evento changeLanguage, pero todavía no lo tenemos programado
    public void ChangeLanguage(){
        //Acá va la logica respectiva del lenguaje

        //TODO: Esto es el evento
        // Analytics.CustomEvent("change_language", new Dictionary<string, object>{
        //     {"Language", language}
        // });
    }
}
