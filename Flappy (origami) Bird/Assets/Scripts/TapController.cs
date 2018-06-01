// Styr fågelns egenskaper och rörelser

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;         // Styr hur mycket fågeln rör sig vid ett musklick
    public float tiltSmooth = 5;        // Styr storleken på tiltninen av fågeln víd ett musklick
    public Vector3 startPos;            // Sätter fågelns startposition

    public AudioSource TapAudio;        // Lägger in ljudfilen "Tap" som spelas vid musklick
    public AudioSource pointAudio;      // Lägger in ljudfilen "point" som spelas när fågeln passerar en scoreZone
    public AudioSource dedAudio;        // Lägger in ljudfilen "ded" som spelas när fågeln passaerar en deadZone

    Rigidbody2D Rigidbody;              // Rigidbody gör så att fågeln kan styras av spelaren 
    Quaternion downRotation;            // Fågelns rotation nedåt
    Quaternion forwardRotation;         // Fågelns rotation framåt
    GameManager game;

    void Start() {
        Rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);             // Styr rotation kring z-axeln
        forwardRotation = Quaternion.Euler(0, 0, 35);           // Styr rotation kring z-axeln
        game = GameManager.Instance;
        Rigidbody.simulated = false;                            // Spelaren kan inte styra fågeln
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        Rigidbody.velocity = Vector3.zero;          // När spelet startar har fågeln hastigheten noll
        Rigidbody.simulated = true;                 // Spelaren kan styra fågeln
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;           //När spelaren dör, återgår fågeln till startpositionen
        transform.rotation = Quaternion.identity;     // Fågeln är fryst i startpositionen
    }

    void Update() {
        if (game.GameOver) return;   

        if (Input.GetMouseButtonDown(0))
        {
            TapAudio.Play();                            // Spelar ljudet Tap vid ett musklick
            transform.rotation = forwardRotation;       // gör att fågeln roterar framåt
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }


   void OnTriggerEnter2D(Collider2D col){
if (col.gameObject.tag == "scoreZone"){
                                                  // Registrerar att spelaren har fått ett poäng
        OnPlayerScored();                         // Skickar händelsen till GameManager
                                                  // Spelar ett ljud när händelsen registrerats
            pointAudio.Play();                    // Ljudet "point" spelas upp
    }

if (col.gameObject.tag == "deadZone")
    {
        Rigidbody.simulated = false;               // Spelaren kan inte styra fågeln 
                                                   // Registrerar att spelaren har dött
        OnPlayerDied();                            // Skickar händelsen till GameManager
                                                   // Spelar ett ljud när händelsen registrerats 
            dedAudio.Play();                       // Ljudet "ded" spelas upp 
    }
}
}