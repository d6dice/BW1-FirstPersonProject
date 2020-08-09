using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;   //Referentie naar de 'Character Controller' genaamd 'controller'

    //Waarom public? Public houd in dat je het kan aanpassen in Unity.

    public float speed = 12f; //Dit is de standaard value voor hoe snel je beweegt
    public float gravity = -10f; //Dit is de standaard gravity value voor hoe snel je valt van hoogtes -10 is standaard.
    
    public float jumpHeight = 8f; // Dit is de standaard value voor de jumpHeight
    public float jetpackStrength = 4f;
    public float jetpackFuel = 0f;
    public float jetpackFuelmaximum = 1f;
    public float jetpackFuelminimum = 0.5f;
    public float deathSpeed = -10f;

    public Transform groundCheck; //Dit is de referentie naar groundCheck in the player sectie van Unity. Dit zorgt er voor dat de Y axis in unity niet eindeloos doortellen
    public float groundDistance = 0.4f; //Dit is de radius van de sphere om de character te laten landen zonder dat de Y axis in unity eindeloos doortellen
    public LayerMask groundMask; //Dit checked welke objecten het op moet letten
    public float death; //vallues op y axis om te bepalen hoe ver je valt voor respawn

    public Canvas pauseScreen;
    private bool paused = false;

    Vector3 velocity; //slaat de huidige 'velocity' op

    bool isGrounded; // Dit slaat op of het op de grond staat
    bool midAirJump; // Dit is een tweede sprong na je eerste sprong
    bool gotJetpack; // Dit is wanneer je de Jetpack op pakt
    bool midAirJetpack; // Dit is dat je na een jump je jetpack activeerd
    bool jetpackFiring; // Dit is dat je na dat je je jetpack hebt geactiveerd je het of ingedrukt kunt houden of het achter elkaar kunt indrukken
    bool deathOnImpact;

    // Start is called before the first frame update
    void Start()
    {
        gotJetpack = false;
        deathOnImpact = false;

        SetPause(paused);
    }

    public void SetPause(bool paused)
    {
        this.paused = paused;

        Time.timeScale = paused ? 0 : 1;
        pauseScreen.enabled = paused;
        UnityEngine.Cursor.visible = paused;
        UnityEngine.Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            SetPause(!paused);
        }

        if (paused)
        {
            return;
        }

        //Deze code zorgt er voor dat de velocity (gravity) word gereset nadat je land ingame

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Dit zijn de inputs voor beweging alle kanten op, standaard zijn de W,A,S,D keys
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //Dit geeft de beweging aan waar we in willen bewegen gebasseerd axis.

        //De gehele codering voor jump en double jump. Auto input is spacebar.

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //Dit zorgt voor de hoogte van springen
                if (gotJetpack == false)
                {
                    midAirJump = true;
                }
                else
                {
                    midAirJetpack = true;
                }
            }
            else if (midAirJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                midAirJump = false;
            }
            else if (midAirJetpack)
            {
                jetpackFiring = true;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            jetpackFiring = false;
        }

        controller.Move(move * speed * Time.deltaTime); //Dit zorgt voor de beweging van de player

        //Deze code zorgt voor gravity

        //"Time.deltaTime" zorgt er voor dat systemen met een lagere FPS (frames per second) voldoende worden geupdate;
        //Dus systeem met hogere framerate zal niet sneller zijn dan een systeem met lagere framerate en visa versa

        velocity.y += gravity * Time.deltaTime; //Dit voegt de velocity values toe ingame

        controller.Move(velocity * Time.deltaTime); //Dit voegt bewegings velocity toe afhankelijk waar de player is

        //Terug plaatsen op de begin positie als je te laag komt
        if (transform.position.y < death)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (jetpackFiring)
        {
            velocity.y = velocity.y + jetpackStrength * Time.deltaTime;
            jetpackFuel = jetpackFuel - Time.deltaTime;
            if (jetpackFuel < 0)
            {
                gotJetpack = false;
                jetpackFiring = false;
            }
        }

        if (velocity.y < deathSpeed)
        {
            deathOnImpact = true;
        }

        if (isGrounded && deathOnImpact)
        {
            SceneManager.LoadScene("RestartScene");
        }

    }
    //Jetpack script
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Jetpack")
        {
            gotJetpack = true;
            jetpackFuel = jetpackFuel + jetpackFuelminimum;
            if (jetpackFuel > jetpackFuelmaximum)
            {
                jetpackFuel = jetpackFuelmaximum;
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "Finish")
        {
            SceneManager.LoadScene("FinishScene");
        }       
        if (other.tag == "DeathFloor")
        {
            SceneManager.LoadScene("RestartScene");
        }
    }
}
