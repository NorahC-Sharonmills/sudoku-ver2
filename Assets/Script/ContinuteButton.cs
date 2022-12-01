using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinuteButton : MonoBehaviour
{
    public Text timeText;
    public Text levelText;

    void Start()
    {
        if (Config.GameDataFileExit() == false)
        {
            this.GetComponent<Button>().interactable = false;
            timeText.text = " ";
            levelText.text = "";
        }
        else
        {
            float delta_time = Config.ReadGameTime();
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string hour = LeadingZero(span.Hours);
            string minute = LeadingZero(span.Minutes);
            string second = LeadingZero(span.Seconds);

            timeText.text = $"{hour}:{minute}:{second}";
            levelText.text = $"Level:{Config.ReadBoardLevel()}";
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void ContinuteGame()
    {
        GameSettings.Instance.SetContinutePreviousGame(true);
        GameSettings.Instance.SetGameMode(Config.ReadBoardLevel());
        SceneManager.LoadScene("Game");
    }
}
