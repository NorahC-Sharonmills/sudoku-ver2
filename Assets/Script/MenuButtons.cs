using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadEasyGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.EASY);
        LoadScene(name);
    }

    public void LoadMediumGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.MEDIUM);
        LoadScene(name);
    }

    public void LoadHardGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.HARD);
        LoadScene(name);
    }

    public void LoadVeryHardGame(string name)
    {
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.VERY_HARD);
        LoadScene(name);
    }

    public void ActiveObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void DeActiveObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Pause()
    {
        GameSettings.Instance.SetPause(true);
        UIHelper.FindScript<PopupPause>().Show();
    }
}
