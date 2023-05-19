/******************************************************************************
// File Name :        GameController.cs
// Author :            Nick Grinstead, Michael Vietti
// Creation Date :     October 3rd, 2022
//
// Brief Description : This script handles a few miscellaneous functions necessary
                       for the game to run.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //Michael wrote down this code 10/5/2022
    public string currentScene;

    // Tracks where the player should start when a scene loads
    static Vector2 spawnPos;
    public Vector2 SpawnPos { get => spawnPos; set => spawnPos = value; }

    /// <summary>
    /// Sets certain values
    /// </summary>
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        // Nick coded this
        // Makes the mouse invisible when not on the main or settings menus
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.visible = false;
        }
    }
}
