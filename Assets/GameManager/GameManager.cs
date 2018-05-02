using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public StateGamePlaying stateGamePlaying { get; set; }
    public StateGameWon stateGameWon { get; set; }
    public StateGameLost stateGameLost { get; set; }
    public StateMainMenu stateMainMenu { get; set; }
    public int Highscore { get; set; }

    private GameState currentState { get; set; }
    public static GameManager Instance { get { return instance; } }

    private static GameManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return; ;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        stateGamePlaying = new StateGamePlaying(this);
        stateGameWon = new StateGameWon(this);
        stateGameLost = new StateGameLost(this);
        stateMainMenu = new StateMainMenu(this);
    }

	// Use this for initialization
	void Start () {
	
        //start in our initial game state, whatever one that is
        NewGameState(stateMainMenu);
	}
	
	// Update is called once per frame
	void Update () {
        
        if(currentState != null) {currentState.StateUpdate();}

	}

    private void OnGUI()
    {
        if (currentState != null) { currentState.StateGUI(); }
    }

    public void NewGameState(GameState newState)
    {
        if(null != currentState)
        {
            currentState.OnStateExit();
        }
        currentState = newState;
        currentState.OnStateEntered();
    }

}

public class StateMainMenu : GameState
{

    
    public StateMainMenu(GameManager manager) : base(manager) { }

    public override void OnStateEntered() { SceneManager.LoadScene("MainMenu"); }
    public override void OnStateExit() { }
    public override void StateUpdate() { }
    public override void StateGUI()
    {
               

    }


}

public class StateGameLost : GameState
{
    public StateGameLost(GameManager manager) : base(manager) { }

    public override void OnStateEntered() { SceneManager.LoadScene("GameOver"); }
    public override void OnStateExit() { }
    public override void StateUpdate() {  }
    public override void StateGUI()
    {
       
    }
    private void BackToMainMenu()
    {
    
       
    }
}

public class StateGameWon : GameState
{
    public StateGameWon(GameManager manager) : base(manager) { }

    public override void OnStateEntered() { }
    public override void OnStateExit() { }
    public override void StateUpdate() { }
    public override void StateGUI()
    {
        
    }
}

public class StateGamePlaying : GameState
{
    private bool isPaused = false;

    public StateGamePlaying(GameManager manager) : base(manager) { }

    public override void OnStateEntered() { SceneManager.LoadScene("Game");  }
    public override void OnStateExit() { }
    public override void StateUpdate()
    {

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGameMode();
            }
            else
            {
                PauseGameMode();
            }
        }
    }
    public override void StateGUI()
    {
        if (isPaused)
        {
          
        }
        else
        {

        }
    }

    private void ResumeGameMode()
    {
        Time.timeScale = 1.0f;
        isPaused = false;
    }
    private void PauseGameMode()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
    }
    
}

