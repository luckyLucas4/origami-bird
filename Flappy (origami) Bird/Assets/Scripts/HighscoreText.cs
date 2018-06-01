// Styr beräkningen och utskriften av highscore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour {

    Text highscore;

    void OnEnable ()        //OnEnable mellan void och Start, texten måste uppdateras varje gång startsidan är aktiverad
    {
        highscore = GetComponent<Text>();
        highscore.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();           // Skriver ut "High Score" på skärm
    }
}

