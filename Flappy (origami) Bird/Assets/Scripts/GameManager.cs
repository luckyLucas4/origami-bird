// Styr vilken av filerna ur Canvasen som visas vid olika skeden i spelet

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public delegate void GameDeligate();
    public static event GameDeligate OnGameStarted;
    public static event GameDeligate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;            // Spelets startsida 
    public GameObject gameOverPage;         // Sidan som visas när spelaren dör
    public GameObject countdownPage;        // Nedräkningen till start
    public Text scoreText;                  // Poängen

    enum PageState
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;                           // Variabel som beskriver poängställning
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } }
    public int Score { get{return score;} }

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable()
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished()                  // Styr vad som händer när nedräkningen är klar
    {
        SetPageState(PageState.None);           // Ingen av sidorna som ligger i Canvasen är aktiverad
        OnGameStarted();                        // Skickar händelsen att spelet startat till TapController
        score = 0;                              // Poängräkningen är noll vid start
        gameOver = false;                       // Spelaren är inte död
    }

    void OnPlayerDied()                                     // Styr vad som händer när spelaren dör
    {
        gameOver = true;                                    // Spelaren har dött
        int savedScore = PlayerPrefs.GetInt("HighScore");   
        if (score > savedScore)                              // Kontrollerar om det har blivit ett nytt highscore eller inte
        {
            PlayerPrefs.SetInt("HighScore", score);         // Nya highscoret skrivs ut
        }
        SetPageState(PageState.GameOver);                   // gameOver sidan aktiveras
    }

    void OnPlayerScored()                        // När fågeln passerar scorezone (mellan två pipes) ökar poängen
    {
        score++;                                 // Poängen ökar med ett
        scoreText.text = score.ToString();       // Poängställningen omvandlas till text och skrivs ut på skärm
    }

    void SetPageState(PageState state)          // Styr vilken sida som visas när spelet körs
    {
        switch (state)
        {
            case PageState.None:
                startPage.SetActive(false);             // Varken startsidan, nedräkningen eller gameOver visas
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;

            case PageState.Start:                       // Styr vad som visas vid spelets start
                startPage.SetActive(true);              // Startsidan är aktiverad
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);           
                break;

            case PageState.GameOver:                    // Styr vad som visas när spelaren dör
                startPage.SetActive(false);
                gameOverPage.SetActive(true);           // gameOver sidan är aktiverad
                countdownPage.SetActive(false);
                break;

            case PageState.Countdown:                   // Styr vad som visas när nedräkningen sker
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);          // countdownPage är aktiverad
                break;
        }
    }

    public void ComfirmGameOver()           // Akiveras när omstartsknapp har blivit klickad på 
    {
       OnGameOverConfirmed();               // Skickar händelsen (spelaren har dött) till TapController
       scoreText.text = "0";                // Poängräkningen sätts till noll
       SetPageState(PageState.Start);       // Startsidan visas återigen
    }

    public void StartGame()                    // Akiveras när startknappen klickas på
    {
        SetPageState(PageState.Countdown);     // Nedräkningssidan visas och nedräkningen börjar
    }
}