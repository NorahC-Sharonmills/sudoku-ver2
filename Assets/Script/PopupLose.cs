using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : UICanvas
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

    public void Continute()
    {
        AdsManager.Instance.ShowReward(() =>
        {
            Hide();
            Lives.Instance.ResetLives();
            Clock.Instance.StartClock();
        }, () =>
        {
            Exit();
        });
    }
}
