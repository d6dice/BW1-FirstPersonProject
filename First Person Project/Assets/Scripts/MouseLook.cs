using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour

{
    public float mouseSensitivity = 100f; //De waarden voor de gevoeligheid van de muis

    public Transform playerBody;

    float xRotation = 0f;
  
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Dit verbergt en 'locked' de muis ingame.
    }

  
    void Update()
    {
        //"Time.deltaTime" zorgt er voor dat systemen met een lagere FPS (frames per second) voldoende worden geupdate;
        //Dus systeem met hogere framerate zal niet sneller zijn dan een systeem met lagere framerate

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; //Dit is de functie die zorgt om horizontaal de camera te bewegen met de muis. 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; //Dit is de functie die zorgt om verticaal de camera te bewegen met de muis.

        //De verticale camera beweging en instellingen voor rotation:

        xRotation -= mouseY; //verminderd de verticale beweging in de - waarde omdat anders de camera flipt
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Dit zorgt er voor dat we nooit te ver verticaal kunnen draaien zodat we niet achter de 'player' kunnen kijken

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //voegt rotation toe zodat we ook de clamp kunnen toevoegen
        playerBody.Rotate(Vector3.up * mouseX);  //Functie om verticaal te blijven bewegen gebasseerd op "Mouse X"
    }
}
