using System;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private float startTime = 120f;

    private float currentTime = 0f;

    private void Update()
    {
        if (startTime > 0)
        {
            startTime -= Time.deltaTime;
        }
        else
        {
            startTime = 0f;
        }
        
        DisplayTime(startTime);
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0f)
        {
            timeToDisplay = 0f;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float miliseconds = Mathf.FloorToInt(timeToDisplay * 1000);
        miliseconds = miliseconds % 1000;

        timerText.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, miliseconds);
    }
}
