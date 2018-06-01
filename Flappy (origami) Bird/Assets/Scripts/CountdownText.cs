// Styr nedräkningen till att spelet startar

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CountdownText : MonoBehaviour {

    public delegate void CountdownFinished();
    public static event CountdownFinished OnCountdownFinished;

    Text countdown;

    void OnEnable()
    {
        countdown = GetComponent<Text>();
        countdown.text = "3";                   // Skriver ut siffan 3 på skärmen när nedräkningen börjar
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        int count = 3;                          // Variabel till nedräkning sätts till tre för att sedan gå till två och avslutningsvis ett
        for (int i = 0; i < count; i++)
        {
            countdown.text = (count - i).ToString();
            yield return new WaitForSeconds(1);
        }
        OnCountdownFinished();                  // Nedräkningen är klar
    }
}
