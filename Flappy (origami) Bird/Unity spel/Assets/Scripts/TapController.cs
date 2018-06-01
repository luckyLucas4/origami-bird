using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate  OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public GameObject test;

    public AudioSource TapAudio;
    public AudioSource pointAudio;
    public AudioSource dedAudio;

    Rigidbody2D Rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;

    void Start() {
        Rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        Rigidbody.simulated = false;
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
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;         //När spelaren dör, återgår fågeln till startpositionen
        transform.rotation = Quaternion.identity;
    }
    void Update() {
        if (game.GameOver) return;
       

        if (Input.GetMouseButtonDown(0))
        {
            TapAudio.Play();
            transform.rotation = forwardRotation;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }


   void OnTriggerEnter2D(Collider2D col){
if (col.gameObject.tag == "scoreZone"){
        // register a score event
        OnPlayerScored(); //event sent to GameMananger;
                          // play a sound
            pointAudio.Play();
    }
if (col.gameObject.tag == "deadZone")
    {
        Rigidbody.simulated = false;
        //register a dead event
        OnPlayerDied(); // event sent to GameMananger;
                        // play a sound
            dedAudio.Play();
    }
}
}