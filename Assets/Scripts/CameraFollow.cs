using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset = new Vector3(0.3f, 0.0f, -10f);
    private Vector3 velocity = Vector3.zero;
    private float dampTime = 0.3f;


    private void Awake() {
        //Le indica a Unity que renderice a un determinado nro de frames
        Application.targetFrameRate = 60;
    }

    public void ResetCameraPosition(){
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);

        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));

        Vector3 destination = point + delta;

        destination = new Vector3(target.position.x, offset.y, offset.z);

        //Esto varia respecto al update porque acá llevo rapido la camara a la nueva posicion. Esto es para cuando se resetea el nivel
        this.transform.position = destination;
    }

    private void Update() {
        //No puedo usar una variable camera con getcomponent porque si yo la creo queda con un valor estatico
        //Entonces de la linea 22 a la 24, el personaje se movio pero la variable de la camara no, por lo tanto
        //Necesito buscar siempre con getcomponent para sacar la pos actual

        //Une las coordenadas del personaje con las de la camara, ya que se manejan diferente
        Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);

        Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(offset.x, offset.y, point.z));

        Vector3 destination = point + delta;

        //Esto evita que la camara se mueva en Y
        destination = new Vector3(target.position.x, offset.y, offset.z);

        //Esto mueve la camara de forma continua y suavizada
        this.transform.position = Vector3.SmoothDamp(this.transform.position, destination, ref velocity, dampTime);
    }


}
