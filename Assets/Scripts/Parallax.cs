using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    //Si tuviera mas fondos, le iria agregando la velocidad a cada uno
    [SerializeField] private float speed = 0.0f;
    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        this.rigidBody.velocity = new Vector2(speed, 0f);

        float parentPosition = this.transform.parent.transform.position.x;

        //20.45f es la medida que se fue de la pantalla y quiero que desaparezca para volver al inicio
        if(this.transform.position.x - parentPosition >= 17.6f){
            ///-20.45f es la medida a la que quiero que vuelva
            this.transform.position = new Vector3(parentPosition -17.6f, this.transform.position.y, this.transform.position.z);
        }
    }
}
