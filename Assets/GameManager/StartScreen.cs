using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {


    public GameManager gameManager;
    public AudioClip start ;
    private AudioSource sourceStart;
    public AudioClip backgroundMusic;
    private AudioSource sourceBackgroundMusic;
    private float startTimeStamp = 0;
    private bool gameToStart = false;

    // Use this for initialization
    void Start () {
        //gameManager = GetComponent<GameManager>();
        gameManager = FindObjectOfType<GameManager>();
        sourceStart = GetComponent<AudioSource>();
        sourceBackgroundMusic = GetComponent<AudioSource>();
        sourceBackgroundMusic.PlayOneShot(backgroundMusic);

    }

// Update is called once per frame
void Update () {
        if (gameToStart == true)
        {
            if (Time.time > startTimeStamp + 3)
            {
                gameManager.NewGameState(gameManager.stateGamePlaying);
            }


        }
        

    }

    public void startGame()
    {
        sourceBackgroundMusic.Stop();
        sourceStart.PlayOneShot(start);
        startTimeStamp = Time.time;
        gameToStart = true;
    }

    public void Exit()
    {

        Application.Quit();
    }
}
