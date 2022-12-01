using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoSingleton<Lives>
{
    public List<GameObject> error_images;

    private int lives_count = 0;
    private int error_number = 0;

    void Start()
    {
        lives_count = error_images.Count;
        error_number = 0;

        if(GameSettings.Instance.GetContinutePreviousGame())
        {
            error_number = Config.ErrorNumber();
            lives_count = error_images.Count - error_number;

            for(int error = 0; error < error_number; error++)
            {
                error_images[error].SetActive(true);
            }
        }
    }

    private void WrongNumber()
    {
        if (error_number < error_images.Count)
        {
            error_images[error_number].SetActive(true);
            error_number++;
            lives_count--;
        }

        CheckForGameLose();
    }

    public int GetErrorNumber() { return error_number; }

    private void CheckForGameLose()
    {
        if (lives_count <= 0)
        {
            GameEvents.OnGameLoseMeThod();
            UIHelper.FindScript<PopupLose>().Show();
        }
    }

    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }

    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }
}
