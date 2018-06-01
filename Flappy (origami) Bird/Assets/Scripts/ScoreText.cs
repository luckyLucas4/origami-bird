// Skriver ut "Game Over" på skärmen när spelaren dör

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    Text score;

    void Start()
    {
        score = GetComponent<Text>();
        score.text = "Game Over";           // När spelaren dör visas texten "Game Over" på skärmen

        // score.text = "Score: " + GameManager.Instance.Score;   Gör så att poängen visas på skärm när spelaren dör, onödigt
    }

}
