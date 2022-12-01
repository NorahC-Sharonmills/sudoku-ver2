using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoSingleton<Clock>
{
    int hour_ = 0;
    int minute_ = 0;
    int second_ = 0;

    private Text textClock;
    private float delta_time;
    private bool stop_clock = false;

    protected override void Awake()
    {
        base.Awake();
        textClock = this.GetComponent<Text>();
    }

    void Start()
    {
        delta_time = 0;
        stop_clock = false;
        if (GameSettings.Instance.GetContinutePreviousGame())
            delta_time = Config.ReadGameTime();
    }

    void Update()
    {
        if (stop_clock == false && GameSettings.Instance.GetPause() == false)
        {
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string _hour = LeadingZero(span.Hours);
            string _minute = LeadingZero(span.Minutes);
            string _second = LeadingZero(span.Seconds);

            textClock.text = $"{_hour}:{_minute}:{_second}";
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    private void OnGameLose()
    {
        stop_clock = true;
    }

    private void OnEnable()
    {
        GameEvents.OnGameLose += OnGameLose;
    }

    private void OnDisable()
    {
        GameEvents.OnGameLose -= OnGameLose;
    }

    public Text GetCurrentTimeText()
    {
        return textClock;
    }

    public static string GetCurretTime()
    {
        return Instance.delta_time.ToString();
    }    

    public void StartClock()
    {
        stop_clock = false;
    }
}