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
        if (Config.GameDataFileExit())
            Config.DeleteDataFile();
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.EASY);
        LoadScene(name);
    }

    public void LoadMediumGame(string name)
    {
        if (Config.GameDataFileExit())
            Config.DeleteDataFile();
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.MEDIUM);
        LoadScene(name);
    }

    public void LoadHardGame(string name)
    {
        if (Config.GameDataFileExit())
            Config.DeleteDataFile();
        GameSettings.Instance.SetGameMode(GameSettings.EGameMode.HARD);
        LoadScene(name);
    }

    public void LoadVeryHardGame(string name)
    {
        if (Config.GameDataFileExit())
            Config.DeleteDataFile();
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

    public void Back()
    {
        AdsManager.Instance.ShowInter(() =>
        {
            GameSettings.Instance.SetExitAfterWon(false);
            GameEvents.OnSaveBoardDataMethod();
            GameSettings.Instance.SetPause(false);
            GameSettings.Instance.SetGameMode(GameSettings.EGameMode.NOT_SET);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        });
    }

    public void Hint()
    {
        AdsManager.Instance.ShowReward(() =>
        {
            GameEvents.OnGiveAHintMethod();
        }, () =>
        {

        });
    }    
}
