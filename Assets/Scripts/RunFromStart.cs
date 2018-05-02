using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RunFromStart : MonoBehaviour {

    public GameManager gameManager;

    void awake()
    {
 

    }
	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("BootScene");

        gameManager = FindObjectOfType<GameManager>();
        gameManager.NewGameState(gameManager.stateMainMenu);

        Destroy(this);



    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Debug.Log("crap");

        }

    }
}
