/******************************************************************************
// File Name :         StarburstRotator.cs
// Author :            Nick Grinstead
// Creation Date :     November 16th, 2022
//
// Brief Description : This script rotates the starburst effects behind each
                       end of level goal and the one on the winscreen.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarburstRotator : MonoBehaviour
{
    /// <summary>
    /// Rotates the attached game object each physics update
    /// </summary>
    private void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0, 0, 45) * Time.deltaTime;
    }
}
