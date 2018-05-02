using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {


    public GameManager gameManager;
    public AudioClip backgroundMusic;
    private AudioSource sourceBackgroundMusic;

    public Text One;
    public Text Two;
    public Text Three;
    public Text FinalMessage;
    public int HighScore;
    public int HighTemp;
    public int HighScorePlacement = 0;
    public int[] allTimeHighScore;
    // Use this for initialization
    void Start () {
        allTimeHighScore = new int[3];
        allTimeHighScore[0] = PlayerPrefs.GetInt("HighScore One");
        allTimeHighScore[1] = PlayerPrefs.GetInt("HighScore Two");
        allTimeHighScore[2] = PlayerPrefs.GetInt("HighScore Three");
        gameManager = FindObjectOfType<GameManager>();
        sourceBackgroundMusic = GetComponent<AudioSource>();
        sourceBackgroundMusic.PlayOneShot(backgroundMusic);
        HighScore = gameManager.Highscore;
        HighTemp = HighScore;

        for (int i = 0; i < allTimeHighScore.Length; i++)
        {
            int temp = allTimeHighScore[i];
            if (HighScore > allTimeHighScore[i])
            {
                allTimeHighScore[i] = HighScore;
                HighScore = temp;
                if (HighScorePlacement == 0)
                {
                    HighScorePlacement = i + 1;

                }


            }


        }

        PlayerPrefs.SetInt("HighScore One", allTimeHighScore[0]);
        PlayerPrefs.SetInt("HighScore Two", allTimeHighScore[1]);
        PlayerPrefs.SetInt("HighScore Three", allTimeHighScore[2]);
        PlayerPrefs.Save();
    }

    



    // Update is called once per frame
    void Update () {

        

        One.text = "1. " + PlayerPrefs.GetInt("HighScore One");
        Two.text = "2. " + PlayerPrefs.GetInt("HighScore Two");
        Three.text = "3. " + PlayerPrefs.GetInt("HighScore Three");

        if (HighScorePlacement == 0)
        {
            FinalMessage.text = "You have not made the top 3";

        }
        else
        {
            FinalMessage.text = "Congratulations, you have placed " + HighScorePlacement + ", in the HighScores List with a score of " + HighTemp; 

        }

    }

    public void tryAgain()
    {
        gameManager.NewGameState(gameManager.stateGamePlaying);

    }

    public void Exit()
    {
        Application.Quit();
       

    }
    public void eraseHighscores()
    {
        PlayerPrefs.DeleteAll();

    }
}



