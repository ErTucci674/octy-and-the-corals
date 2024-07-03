using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PlayerStats : MonoBehaviour
{
    public GameObject pipeSpawner;
    public Text scoreText;
    public Text startText;
    public Text deathScoreText;
    public Text deathHighScoreText;
    private int score = 0;
    private int highScore = 0;
    private const int scoreStart = 0;

    public Text buttonText;
    public GameObject deathScreen;

    public bool died = false;
    private bool diedWait = false;

    private AudioSource pointSound;

    private void Awake()
    {
        // Get the stored high score when scene is loaded
        highScore = PlayerPrefs.GetInt("highScore");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Pause Game and Set player points and button
        Time.timeScale = 0;
        Setup();
        RestartScore();
    }

    // Update is called once per frame
    void Update()
    {
        // If the death screen is up and space key is pressed, Start scene
        if (deathScreen.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            StartButton();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Increment score if player collides with the pipe trigger
        if (collision.gameObject.CompareTag("PipeTrigger"))
        {
            pointSound.Play();
            AddScore(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player dies if they collide with anything but the trigger
        // The 'Die' function is called after set seconds
        died = true;
        diedWait = true;
        StartCoroutine(WaitBeforeDie(1));
    }

    private void Setup()
    {
        died = false;
        deathScreen.SetActive(false);
        buttonText.gameObject.SetActive(true);
        buttonText.text = "OCTY AND THE CORALS";
        startText.gameObject.SetActive(true);

        // Select the second audio source contained in the game object
        pointSound = GetComponents<AudioSource>()[1];
    }

    private void RestartScore()
    {
        score = scoreStart;
        scoreText.text = score.ToString();
    }

    private void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        // Increase difficulty every 5 points/pipes
        if (score % 5 == 0)
        {
            DecreasePipesSpawningTime();
        }
        scoreText.text = score.ToString();
    }

    private void DecreasePipesSpawningTime()
    {
        // The pipes spawning rate and their speed is increased
        var spawner = pipeSpawner.GetComponent<PipeSpawner>();
        if (spawner.spawnTime > 0.06f)
        {
            spawner.spawnTime -= 0.06f;
            spawner.pipeSpeed += 0.1f;
        }
    }

    public void Die()
    {
        // The death screen is shown, game is paused and button text changed
        Time.timeScale = 0;
        scoreText.gameObject.SetActive(false);
        deathScreen.SetActive(true);
        buttonText.gameObject.SetActive(true);
        buttonText.text = "";

        // Save new high score for the following Loaded scenes
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        deathScoreText.text = "SCORE\n" + score.ToString();
        deathHighScoreText.text = "HIGH SCORE\n" + highScore.ToString();

        diedWait = false;
    }

    IEnumerator WaitBeforeDie(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Die();
    }

    public void StartButton()
    {
        // Start button function changes depending on player's status
        // The game is reloaded if player is dead
        if (died && !diedWait)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Death screen is removed and game unpaused if player is alive
        else
        {
            deathScreen.SetActive(false);
            buttonText.gameObject.SetActive(false);
            startText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
