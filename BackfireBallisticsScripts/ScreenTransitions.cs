/******************************************************************************
// File Name :         ScreenTransitions.cs
// Author :            Nick Grinstead, Peter Campbell
// Creation Date :     October 12, 2022
//
// Brief Description : This script tracks the current scene the player is in and
                       loads a new scene when prompted.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitions : MonoBehaviour
{
    static int sceneIndex;

    [SerializeField] GameController gc; 

    [SerializeField] Vector2 nextSpawn;

    /// <summary>
    /// Ensures that sceneIndex matches the current scene
    /// </summary>
    private void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    /// <summary>
    /// To be called by other scripts to load a new scene
    /// </summary>
    /// <param name="newIndex">Index of scene to be loaded</param>
    /// <param name="isReset">True if scene is being loaded by reset button</param>
    public void LoadScene(int newIndex, bool isReset)
    {
        sceneIndex = newIndex;

        // Only sets a new spawn if a level was completed
        if (!isReset)
        {
            gc.SpawnPos = nextSpawn;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
