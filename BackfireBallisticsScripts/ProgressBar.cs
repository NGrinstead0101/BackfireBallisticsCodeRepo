/******************************************************************************
// File Name :         ProgressBar.cs
// Author :            Michael Vietti, Nick Grinstead
// Creation Date :     October 24th, 2022
//
// Brief Description : This script creates a progress icon for reloading and 
                       updates the UI for tracking shots.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float minimum;
    public float maximum;
    public float current;
    public Image mask;
    public Image fill;
    public Color color;

    // Coded by Nick
    [SerializeField] GameObject[] shotCountIcons = new GameObject[3];

    /// <summary>
    /// This script manages the progress bar for reloading
    /// </summary>
    /// <param name="currentIncrement">The current progress of the reload</param>
    /// <param name="shotCount">The number of times the player has shot</param>
    public void GetCurrentFill(float currentIncrement, int shotCount)
    {
        current = currentIncrement;
        if (shotCount == 3)
        {
            float currentOffset = current - minimum;
            float maximumOffset = maximum - minimum;
            float fillAmount = currentOffset / maximumOffset;
            mask.fillAmount = fillAmount;
        }

        fill.color = color;
    }

    // Coded by Nick
    /// <summary>
    /// This script updates the UI icons that track how many shots the player has
    /// </summary>
    /// <param name="currentCount">The number of times the player has shot</param>
    public void UpdateShotCount(int currentCount)
    {
        for (int i = 0; i < shotCountIcons.Length; i++)
        {
            if (i < currentCount)
            {
                shotCountIcons[i].SetActive(false);
            }
            else
            {
                shotCountIcons[i].SetActive(true);
            }
        }
    }
}
