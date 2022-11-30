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
        GameSettings.Instance.SetPause(false);
        base.Hide();
    }

    public void Exit()
    {
        Hide();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
