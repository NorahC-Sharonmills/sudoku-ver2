using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> error_images;

    private int lives_count = 0;
    private int error_number = 0;

    void Start()
    {
        lives_count = error_images.Count;
        error_number = 0;
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
