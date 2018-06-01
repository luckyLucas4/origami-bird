using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour {

    Text highscore;

    void OnEnable() //OnEnable mellan void och start, texten måste uppdateras varje gång startsidan är aktiverad
    {
        highscore = GetComponent<Text>();
        highscore.text = "High score: " +PlayerPrefs.GetInt("HighScore").ToString();
    }
}

