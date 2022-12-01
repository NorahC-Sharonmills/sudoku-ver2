using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinIndicator : MonoBehaviour
{
    private void OnBoardComplete()
    {
        UIHelper.FindScript<PopupWin>().Show();
    }

    private void OnEnable()
    {
        GameEvents.OnBoardComplete += OnBoardComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardComplete -= OnBoardComplete;
    }
}
