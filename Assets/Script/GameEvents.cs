using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void UpdateSquareNumber(int number);
    public static event UpdateSquareNumber OnUpdateSquareNumber;

    public static void UpdateSquareNumberMethod(int number)
    {
        if (OnUpdateSquareNumber != null)
            OnUpdateSquareNumber(number);
    }

    public delegate void SquareSelected(int square_index);
    public static event SquareSelected OnSquareSelected;

    public static void UpdateSquareSelectedMethod(int square_index)
    {
        if (OnSquareSelected != null)
            OnSquareSelected(square_index);
    }

    public delegate void WrongNumber();
    public static event WrongNumber OnWrongNumber;

    public static void OnWrongNumberMethod()
    {
        if (OnWrongNumber != null)
            OnWrongNumber();
    }

    public delegate void GameLose();
    public static event GameLose OnGameLose;
    public static void OnGameLoseMeThod()
    {
        if (OnGameLose != null)
            OnGameLose();
    }

    //-------------------------------------
    public delegate void NotesActive(bool active);
    public static event NotesActive OnNotesActive;

    public static void OnNotesActiveMethod(bool active)
    {
        if (OnNotesActive != null)
            OnNotesActive(active);
    }
}
