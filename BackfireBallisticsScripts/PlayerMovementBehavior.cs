/******************************************************************************
// File Name :         PlayerMovementBehavior.cs
// Author :            Aidan O'Donnell, Cole Stranczek, Nick Grinstead, Peter
                       Campbell, Michael Vietti
// Creation Date :     November 9th, 2022
//
// Brief Description : This script handles all the behavior of the player's 
                       movement and controls, from the shotgun firing, to the
                       gun reloading and launch speed
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerMovementBehavior : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private SpriteRenderer sr;

    private Animator anim;

    public GameObject readyToFireIcon;

    public GameObject oneHealthHeart;

    public GameObject twoHealthHeart;

    public GameObject threeHealthHeart;

    public GameObject loseScreen;

    public int heartCount;

    public int shotCount;

    public float playerSpeed;

    public Vector2 recoilForce;

    public bool haveLaunched;

    public bool invincible;

    public string currentScene;

    private bool isFlipped = false;

    private bool onGround = true;
    // Mask for objects considered to be "ground"
    [SerializeField] LayerMask mask;

    // Piece of the reload time, meant to be a small value (such as 0.1)
    //[SerializeField] float reloadTime;
    // Amount to be added to currentIncrement when on ground
    [SerializeField] int groundReloadIncr;
    // Amount to be added to currentIncrement when in the air
    [SerializeField] int airReloadIncr;
    // Amount currentIncrement needs to reach in order for gun to reload
    [SerializeField] int incrementTotal;
    public int currentIncrement = 0;

    [SerializeField] ProgressBar reloadBar;

    // Code by Nick
    // For accessing the player's spawn position
    [SerializeField] GameController gc;

    // For accessing the isGamePaused variable in this script (Cole)
    public PauseScreenBehavior ps;

    public TimerScript ts;

    // Audio Vars (written by Peter)
    public AudioSource playerAudioSource;
    [SerializeField] AudioClip _shotSound;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] AudioClip _hurtSound;
    public AudioClip boostSound;
    [SerializeField] AudioClip _reloadSound;

    // Added by Nick
    private IEnumerator shotDelay;
    private WaitForSeconds reloadTime;

    // Start is called before the first frame update
    void Awake()
    {
        // Coded by Nick
        shotDelay = ShotDelay();
        reloadTime = new WaitForSeconds(0.1f);

        haveLaunched = false;

        invincible = false;

        oneHealthHeart.SetActive(true);
        twoHealthHeart.SetActive(true);
        threeHealthHeart.SetActive(true);
        heartCount = 3;

        shotCount = 0;

        rigidBody = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();

        currentScene = SceneManager.GetActiveScene().name;

        readyToFireIcon.SetActive(true);

        ps = GetComponent<PauseScreenBehavior>();

        ts = GetComponent<TimerScript>();

        playerAudioSource = GetComponent<AudioSource>();

        loseScreen.SetActive(false);

        transform.position = new Vector3(gc.SpawnPos.x, gc.SpawnPos.y, 0); // Edited by Nick
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isWalking", false);

        // Coded by Nick
        // Raycast checks if player is on ground
        RaycastHit2D groundCheck = Physics2D.Linecast(rigidBody.position, 
            rigidBody.position - new Vector2(0, 0.85f), mask);
        // Sets onGround based on raycast results
        if (groundCheck.collider != null)
        {
            onGround = true;
            anim.SetBool("hasLaunched", false);
        }
        else
        {
            onGround = false;
            anim.SetBool("hasLaunched", true);
        }

        //Aidan Coded this
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // Coded by Nick
            // Flips character sprite
            if (isFlipped)
            {
                isFlipped = false;
                sr.flipX = false;
            }

            anim.SetBool("isWalking", true);
            // Only allows movement if tthe player isn't on a sticky wall
            if (transform.parent == null)// Coded by Nick
            {
                transform.position = transform.position + Vector3.right
                * Time.deltaTime * playerSpeed;
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // Coded by Nick
            // Flips player sprite
            if (!isFlipped)
            {
                isFlipped = true;
                sr.flipX = true;
            }

            anim.SetBool("isWalking", true);

            // Only allows movement if the player isn't on a sticky wall
            if (transform.parent == null)// Coded by Nick
            {
                transform.position = transform.position + Vector3.left
                * Time.deltaTime * playerSpeed;
            }
        }

        // Nick coded this
        // Resets the players drag when a launch has ended
        if (Mathf.Abs(rigidBody.velocity.x) <= 5 && Mathf.Abs(rigidBody.velocity.y) <= 5)
        {
            invincible = false;
        }

        // Cole is coding this
        if (Input.GetMouseButtonDown(0)
            && shotCount < 3 && ps.isGamePaused == false)
        {
            shotCount++;
            reloadBar.UpdateShotCount(shotCount);// Coded by Nick

            haveLaunched = true;
            anim.SetBool("hasLaunched", true);

            invincible = true;

            StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake());
        }
        // Cole
        if (Mathf.Abs(rigidBody.velocity.x) > 15 || Mathf.Abs(rigidBody.velocity.y) > 15)
        {
            invincible = true;
        }

    }

    // Coded by Nick
    /// <summary>
    /// Handles the physics based movement for shooting
    /// </summary>
    private void FixedUpdate()
    {
        if (haveLaunched)
        {
            haveLaunched = false;

            // Determine launch direction
            Vector3 mousePosition =
               Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 launchDirection = mousePosition - transform.position;

            // Calculate force to be applied
            Vector2 launchForce = recoilForce * launchDirection.normalized;

            Vector2 newVelocity = rigidBody.velocity + launchForce;

            newVelocity = Vector2.ClampMagnitude(newVelocity, 20);

            rigidBody.velocity = newVelocity;

            //Unfreezes (implement by Peter)
            UnFreeze();

            // Plays shotgun sound (added by Peter)
            playerAudioSource.PlayOneShot(_shotSound, .2f);

            // Handles the reloading of the gun
            StartCoroutine(shotDelay);
        }
    }

    /// <summary>
    /// Controls delay between shots
    /// Written by Peter, Modified by Nick
    /// </summary>
    /// <returns>Time between reload increments</returns>
    IEnumerator ShotDelay()
    {
        currentIncrement = 0;

        while (true)
        {
             //If total has been reached, the player reloads
            if (currentIncrement >= incrementTotal)
            {
                ResetHaveLaunched();

                // Reload sound (implemented by Peter)
                playerAudioSource.PlayOneShot(_reloadSound, 10);
            }
            // Adjusts currentIncrement based on if player is on ground or in air
            else if (onGround)
            {
                currentIncrement += groundReloadIncr;
            }
            else
            {
                currentIncrement += airReloadIncr;
            }

            reloadBar.GetCurrentFill(currentIncrement, shotCount);

            yield return reloadTime;
        }
    }

    // Nick coded this
    /// <summary>
    /// Resets haveLaunched to allow player to shoot again
    /// </summary>
    public void ResetHaveLaunched()
    {
        shotCount = 0;
        reloadBar.UpdateShotCount(shotCount);
        currentIncrement = 0;

        readyToFireIcon.SetActive(true);
        StopCoroutine(shotDelay);

        readyToFireIcon.SetActive(true);
    }

    /// <summary>
    /// Called when the player collides with a trigger
    /// </summary>
    /// <param name="other">Collider of other object</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Audio implemented by Peter

        /* Restarts the level when the player hits the kill border 
        at the bottom of the screen */
        if (other.tag == "Kill Zone")
        {
            KillPlayer();
        }

        // Deals damage to player
        // Initailly written by Cole, modified by Peter
        if (other.tag == "Enemy")
        {
            if (invincible == false)
            {
                switch (heartCount)
                {
                    case 3:
                        playerAudioSource.PlayOneShot(_hurtSound);
                        threeHealthHeart.SetActive(false);
                        heartCount--;
                        break;

                    case 2:
                        playerAudioSource.PlayOneShot(_hurtSound);
                        twoHealthHeart.SetActive(false);
                        heartCount--;
                        break;

                    case 1:
                        playerAudioSource.PlayOneShot(_hurtSound);
                        oneHealthHeart.SetActive(false);
                        heartCount--;
                        break;

                    case 0:
                        KillPlayer();
                        break;

                }
            }
            else
            {
                playerAudioSource.PlayOneShot(boostSound, 5);
            }
        }
    }

    /// <summary>
    /// Called when the player collides with a non-trigger object
    /// Written by Peter
    /// </summary>
    /// <param name="collision">Object player collides with</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Plays sounds for bouncing
        if (collision.gameObject.tag == "Bounce")
        {
            playerAudioSource.PlayOneShot(boostSound, 5);
        }
    }

    /// <summary>
    /// Unfreezes player if necessary (Written by Peter)
    /// </summary>
    public void UnFreeze()
    {
        // Coded by Nick
        if (rigidBody.gravityScale == 0)
        {
            if (transform.parent != null)
            {
                transform.parent.GetComponent<StickyWall>().IsStuck = false;
            }
            rigidBody.gravityScale = 1;
            transform.SetParent(null);
        }
    }


    /// <summary>
    /// Boosts the player's velocity (Written by Peter)
    /// </summary>
    /// <param name="magnitude">The amount to boost the player's velocity</param>
    public void BoostVelocity(int magnitude)
    {
        rigidBody.velocity += rigidBody.velocity.normalized * magnitude;
    }

    /// <summary>
    /// Activates game over state. Contents written by others, method created
    /// by Peter
    /// </summary>
    void KillPlayer()
    {
        playerAudioSource.PlayOneShot(_deathSound, 3);
        Destroy(GameObject.Find("Music Source"));
        loseScreen.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
