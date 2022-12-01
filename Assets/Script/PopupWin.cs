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
        base.Show();
        textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }

    public void Exit()
    {
        AdsManager.Instance.ShowInter(() =>
        {
            Hide();
            GameSettings.Instance.SetExitAfterWon(true);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        });
    }

    public void Play()
    {
        AdsManager.Instance.ShowInter(() =>
        {
            Hide();
            GameSettings.Instance.SetExitAfterWon(true);
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        });
    }
}
