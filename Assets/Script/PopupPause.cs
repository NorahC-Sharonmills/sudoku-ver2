using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupPause : UICanvas
{
    public Text textClock;

    public override void Show()
    {
        base.Show();
        textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }

    public override void Hide()
    {
        base.Hide();
        GameSettings.Instance.SetPause(false);
    }

    public void Exit()
    {
        base.Hide();
        GameSettings.Instance.SetPause(false);
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.NOT_SET);
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
