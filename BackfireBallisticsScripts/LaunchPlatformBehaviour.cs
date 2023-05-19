/******************************************************************************
// File Name :         LaunchPlatformBehaviour.cs
// Author :            Nick Grinstead
// Creation Date :     September 26, 2022
//
// Brief Description : This script launches the player into the air when they
                       step on a specific platform.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlatformBehaviour : MonoBehaviour
{
    [SerializeField] float launchForce;
    [SerializeField] Rigidbody2D playerRb2d;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb2d.velocity += new Vector2(0, launchForce);
        }
    }
}
