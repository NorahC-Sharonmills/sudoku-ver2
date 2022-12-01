using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupWin : UICanvas
{
    public Text textClock;

    public override void Show()
    {
        Debug.Log("show");
        base.Show();
        textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }

    public void Exit()
    {
        Hide();
        GameSettings.Instance.SetExitAfterWon(true);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Play()
    {
        Hide();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
