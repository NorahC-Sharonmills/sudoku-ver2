using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoSingletonGlobal<GameSettings>
{
    public enum EGameMode
    {
        NOT_SET,
        EASY,
        MEDIUM,
        HARD,
        VERY_HARD
    }

    [SerializeField] private EGameMode _GameMode;
    [SerializeField] private bool _Paused = false;

    private bool _ContinutePreviousGame = false;
    private bool _exitAfterWin = false;

    public void SetExitAfterWon(bool set)
    {
        _exitAfterWin = set;
        _ContinutePreviousGame = false;
    }

    public bool GetExitAfterWon() { return _exitAfterWin; }
    public void SetContinutePreviousGame(bool continute_game)
    {
        _ContinutePreviousGame = continute_game;
    }

    public bool GetContinutePreviousGame() { return _ContinutePreviousGame; }

    public void SetPause(bool _paused = true) { _Paused = _paused; }
    public bool GetPause() { return _Paused; }

    private void Start()
    {
        _Paused = false;
        _GameMode = EGameMode.NOT_SET;
        _ContinutePreviousGame = false;
    }

    public void SetGameMode(EGameMode mode)
    {
        _GameMode = mode;
    }

    public void SetGameMode(string mode)
    {
        switch(mode)
        {
            case "Easy":
                _GameMode = EGameMode.EASY;
                break;
            case "Medium":
                _GameMode = EGameMode.MEDIUM;
                break;
            case "Hard":
                _GameMode = EGameMode.HARD;
                break;
            case "VeryHard":
                _GameMode = EGameMode.VERY_HARD;
                break;
            default:
                _GameMode = EGameMode.NOT_SET;
                break;
        }
    }

    public string GetGameMode()
    {
        switch(_GameMode)
        {
            case EGameMode.EASY: return "Easy";
            case EGameMode.MEDIUM: return "Medium";
            case EGameMode.HARD: return "Hard";
            case EGameMode.VERY_HARD: return "VeryHard";
        }

        Debug.Log("game mode not set");
        return " ";
    }
}
