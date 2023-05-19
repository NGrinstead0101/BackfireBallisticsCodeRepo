/******************************************************************************
// File Name :         ButtonSceneChangeBehavior.cs
// Author :            Aidan O'Donnell, Nick Grinstead
// Creation Date :     September 28, 2022
//
// Brief Description : This script is for changing the scene upon the pressing
                        of a UI button
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ButtonSceneChangeBehavior : MonoBehaviour
{
    //coded by Aidan O'Donnell
    public int sceneSelected;

    public bool settingsOn = false;

    public GameObject mainMenu;

    public GameObject settings;

    [SerializeField] bool isMainMenu;
    [SerializeField] ScreenTransitions st;
    [SerializeField] PauseScreenBehavior psb;
    
    // Coded by Nick
    /// <summary>
    /// Makes cursor visible on main menu
    /// </summary>
    private void Awake()
    {
        if (isMainMenu)
        {
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Changes the scene
    /// </summary>
    public void ChangeScene()
    {
        // Coded by Nick
        if (!isMainMenu)
        {
            psb.Resume();
            st.LoadScene(sceneSelected, true);
        }
        else
        {
            st.LoadScene(sceneSelected, false);
        }
    }

    // Coded by Nick
    /// <summary>
    /// Used by the Quit to Menu Button in the pause screen
    /// </summary>
    public void ReturnToMenu()
    {
        psb.Resume();
        st.LoadScene(0, true);
    }

    /// <summary>
    /// Swaps between the settings menu and the main menu
    /// </summary>
    public void SettingsChange()
    {
        if (settingsOn == false)
        {
            settings.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            settings.SetActive(false);
        }
    }

    //exits the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
