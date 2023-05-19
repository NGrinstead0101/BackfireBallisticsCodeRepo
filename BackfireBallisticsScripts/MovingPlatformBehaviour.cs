/******************************************************************************
// File Name :         MovingPlatformBehaviour.cs
// Author :            Nick Grinstead
// Creation Date :     October 31st, 2022
//
// Brief Description : This script controls the movement of platforms and allows
                       the player to remain on top of them.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 startPos;
    [SerializeField] Vector2 endPos;
    [SerializeField] float xSpeed;
    [SerializeField] float ySpeed;
    //should only be 1 for right or -1 for left
    [SerializeField] int horiz = 1;
    //set to 1 for up and -1 for down
    [SerializeField] int vert = 1;

    Transform playerTransform;
    
    /// <summary>
    /// Ensures horiz and vert are set to valid numbers
    /// </summary>
    void Start()
    {
        if(!(horiz == 1 || horiz == -1))
        {
            horiz = 1;
        }
        if (!(vert == 1 && vert == -1))
        {
            vert = 1;
        }
        transform.position = startPos;
    }

    /// <summary>
    /// Moves the platform each frame
    /// </summary>
    void FixedUpdate()
    {
        Vector3 newPos = transform.position;

        newPos.x += horiz * Time.deltaTime * xSpeed;            
        newPos.y += vert * Time.deltaTime * ySpeed;

        if (startPos.x <= endPos.x)
        {
            newPos.x = Mathf.Clamp(newPos.x, startPos.x, endPos.x);
        }
        else
        {
            newPos.x = Mathf.Clamp(newPos.x, endPos.x, startPos.x);
        }
        if (startPos.y <= endPos.y)
        {
            newPos.y = Mathf.Clamp(newPos.y, startPos.y, endPos.y);
        }
        else
        {
            newPos.y = Mathf.Clamp(newPos.y, endPos.y, startPos.y);
        }

        //Switch direction if platform reaches an endpoint
        if(transform.position == newPos)
        {
            horiz *= -1;
            vert *= -1;
        }

        transform.position = newPos;
    }

    /// <summary>
    /// Makes the player a child of the platform on collision
    /// </summary>
    /// <param name="collision">data from a collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.GetComponent<Transform>();

            playerTransform.SetParent(transform);
        }
    }

    /// <summary>
    /// Makes the player have no parent upon leaving the platform
    /// </summary>
    /// <param name="collision">data from a collision</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform.SetParent(null);
        }
    }
}
