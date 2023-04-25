using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType{
    health,
    energy
}
public class PlayerBar : MonoBehaviour
{
    private Slider slider;

    public BarType type;

    private void Start() {
        this.slider = GetComponent<Slider>();
        //Al ser constantes no necesito el sharedInstance
        switch(this.type){
            case BarType.health:
                this.slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.energy:
                this.slider.maxValue = PlayerController.MAX_ENERGY;
                break;
        }
    }

    private void Update() {
        switch(this.type){
            case BarType.health:
                this.slider.value = PlayerController.sharedInstance.GetHealth();
                break;
            case BarType.energy:
                this.slider.value = PlayerController.sharedInstance.GetEnergy();
                break;
        }
    }
}
