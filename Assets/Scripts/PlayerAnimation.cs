using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerStats stats;
    private SpriteRenderer player;
    public Sprite idle;
    public Sprite swim;
    public Sprite death;

    private float timer;
    private const float animationStay = 0.5f;
    private bool swam = false;

    private AudioSource swimSound;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        player = GetComponent<SpriteRenderer>();
        player.sprite = idle;
        swimSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stats.died)
        {
            CheckUserInput();
            SwimAnimation();
        }
    }

    private void CheckUserInput()
    {
        // Check the user pressed the correct keys
        // If correct, activate swim animation
        if (player.sprite != death && Input.GetMouseButtonDown(0))
        {
            // Play the swimming sound on every click
            swimSound.Play();

            // If the swimming sprite is on already, just reset the animation timer
            if (swam)
            {
                timer = 0;
            }
            else
            {
                swam = true;
                player.sprite = swim;
            }
        }
    }

    private void SwimAnimation()
    {
        // When the animation is 'activated' the sprite is changed from idle to swim
        // A timer keeps track of how long the 'swim' sprite has been shown for
        // When the timer reaches the 'animationStay' value, the sprite is set
        if (swam)
        {
            if (timer >= animationStay)
            {
                player.sprite = idle;
                swam = false;
                timer -= animationStay;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player's sprite changes to 'death' sprite when they collide
        player.sprite = death;
    }
}
