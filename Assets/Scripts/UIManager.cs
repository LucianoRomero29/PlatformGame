using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] imagesAudio;
    [SerializeField] AudioSource audioSourceMusic, audioSourcePlayer;
    private bool muteMusic = false, muteSFX = true;
    public void MuteAndDesmuteMusic(){
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"mute_music", muteMusic}
        };

        AnalyticsService.Instance.CustomData("muteMusic", parameters);
        
        audioSourceMusic.mute = muteMusic;
        muteMusic = !muteMusic;
        if(muteMusic){
            imagesAudio[0].SetActive(true);
            imagesAudio[1].SetActive(false);
        }else{
            imagesAudio[0].SetActive(false);
            imagesAudio[1].SetActive(true);
        } 
    }

    public void MuteAndDesmuteSFX(){
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            {"mute_sfx", muteSFX}
        };
        AnalyticsService.Instance.CustomData("muteSFX", parameters);

        audioSourcePlayer.mute = muteSFX;
        muteSFX = !muteSFX;

        if(muteSFX){
            imagesAudio[0].SetActive(true);
            imagesAudio[1].SetActive(false);
        }else{
            imagesAudio[0].SetActive(false);
            imagesAudio[1].SetActive(true);
        }
    }
}
