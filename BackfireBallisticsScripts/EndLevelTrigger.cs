/******************************************************************************
// File Name :         EndLevelTrigger.cs
// Author :            Nick Grinstead
// Creation Date :     October 12, 2022
//
// Brief Description : This script will be attached to the goal at the end of 
                       each level. It calls the ScreenTransitions script when
                       the player reaches it.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelTrigger : MonoBehaviour
{ 
    [SerializeField] ScreenTransitions st;

    /// <summary>
    /// Checks for player colliding with object
    /// </summary>
    /// <param name="collision">Data from collision</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            st.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, false);
        }
    }
}
