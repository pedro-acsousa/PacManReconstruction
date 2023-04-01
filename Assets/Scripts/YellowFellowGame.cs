using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;
using static Ghost;

public class YellowFellowGame : MonoBehaviour
{
    [SerializeField]
    GameObject highScoreUI;

    [SerializeField]
    GameObject mainMenuUI;

    [SerializeField]
    GameObject pauseUI;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    GameObject gameoverUI;

    [SerializeField]
    GameObject winUI;

    [SerializeField]
    Fellow playerObject;

    [SerializeField]
    Ghost ghost;

    [SerializeField]
    GameObject fruit;



    GameObject[] pellets;
    GameObject[] ghosts;
    GameObject[] powerups;

    private Text gameOverText;
    private Text currentScore;
    private Text highscoreText;
    public InputField field;
    public string nameuser;

    public int level=1;
    bool levelup = false;

    [SerializeField]
    HighScoreTable highscore;

    [SerializeField]
    float fruitCount = 10f;
    [SerializeField]
    public float timeActive = 5f;
  




    enum GameMode
    {
        InGame,
        MainMenu,
        HighScores
    }

    GameMode gameMode = GameMode.MainMenu;

    

    // Start is called before the first frame update
    void Start()
    {
        StartMainMenu();
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        powerups = GameObject.FindGameObjectsWithTag("Powerup");
        Time.timeScale = 0;
        pauseUI.gameObject.SetActive(false);
        gameoverUI.gameObject.SetActive(false);
        highscore.LoadHighScoreTable();
        



    }

    // Update is called once per frame
    void Update()

    {

        fruitCount -= Time.deltaTime;
        if (fruitCount <= 0f)
        {
            fruit.SetActive(true);
            timeActive -= Time.deltaTime;
            if (timeActive <= 0f)
            {
                fruit.SetActive(false);
                timeActive = 5f;
                fruitCount = 13f;
            }
            

        }


        switch (gameMode)
        {
            case GameMode.MainMenu:     UpdateMainMenu(); break;
            case GameMode.HighScores:   UpdateHighScores(); break;
            case GameMode.InGame:       UpdateMainGame(); break;
        }

        if (playerObject.PelletsEaten() == pellets.Length)
        {
            Debug.Log("Level Completed!");
            level= level+1;
            levelup = true;
            RestartGame();
            
        }

      

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pauseUI.gameObject.SetActive(true);

            }
            else {
                Time.timeScale = 1;
                pauseUI.gameObject.SetActive(false);
            }
        }

        if (playerObject.life <= 0) {
            Time.timeScale = 0;
            gameoverUI.gameObject.SetActive(true);

            

            gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            gameOverText.text = "Game Over \n You scored: " + playerObject.score; 

           
        }



}

    void UpdateMainMenu()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
            Time.timeScale = 0;

        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartHighScores();
        }
    }

    void UpdateHighScores()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartMainMenu();
        }
    }

    void UpdateMainGame()
    {
       // playerObject
    }

    void StartMainMenu()
    {
        gameMode                        = GameMode.MainMenu;
        mainMenuUI.gameObject.SetActive(true);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
    }


    void StartHighScores()
    {
        gameMode                = GameMode.HighScores;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
    }

    void StartGame()
    {
        gameMode = GameMode.InGame;
        mainMenuUI.gameObject.SetActive(false);
        highScoreUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameoverUI.gameObject.SetActive(false);

        highscoreText = GameObject.Find("HighScoreTitle").GetComponent<Text>();
        Dictionary<string, int> highest = new Dictionary<string, int>();
        highest = highscore.highscore;
        highscoreText.text = "Highscore: \n" + highest.ElementAt(0).Key.ToString() + "\n " + highest.ElementAt(0).Value.ToString() + " pts";




    }

    public void SaveScore() {
        GameObject inputFieldObject = GameObject.Find("NameText");
        InputField inputFieldText = inputFieldObject.GetComponent<InputField>();
        nameuser = inputFieldText.text;
        highscore.WriteHighScore(nameuser, playerObject.score);
        Application.Quit();



    }

    public void RestartGame() {
        if (!levelup)
        {


            playerObject.life1.SetActive(true);
            playerObject.life2.SetActive(true);
            playerObject.life3.SetActive(true);
            level=1;
            playerObject.life = 3;
            playerObject.score = 0;

        }

        
        foreach (GameObject pellet in pellets) {
            pellet.SetActive(true);
            playerObject.pelletsEaten = 0;
            pellets = GameObject.FindGameObjectsWithTag("Pellet");
        }

        foreach (GameObject powerup in powerups)
        {
            powerup.SetActive(true);
        }

        Text levelText = GameObject.Find("Level").GetComponent<Text>();
        levelText.text = "Level: " + level;

        foreach (GameObject phantom in ghosts) {
            UnityEngine.AI.NavMeshAgent agent;
            agent = phantom.GetComponent<NavMeshAgent>();
            ghost.GhostReset(agent);
            if (levelup)
            {
                ghost.GhostSpeedLevelUp(agent);
                
            }
        }
        if (levelup)
        {
            playerObject.FellowLevelUp();
        }

            gameoverUI.gameObject.SetActive(false);
        Time.timeScale = 1;



        levelup = false;
    }
}
