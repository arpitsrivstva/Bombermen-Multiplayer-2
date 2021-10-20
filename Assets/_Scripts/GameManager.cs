using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerBrain player1;
    public PlayerBrain player2;

    public StateManager stateManager;

    public RectTransform gameStatusText; //will check if both players are ready
    public Text countdownTimerText;
    int countdownTimer = 3;
    float nextCountdownTick = 0;

public AudioSource audioSource;
    public AudioClip countdownTick;
    public AudioClip countdownEnd;
    public AudioClip music;

    float roundStartTime = 0;
    public FloatVariable roundTimer;

    public GameMode gameMode;

    
    // Start is called before the first frame update
    void Awake()
    {
        

        player1.gameManager = this;
        player2.gameManager = this;

        stateManager.gameManager = this;

        stateManager.currentGameState = GameState.PreGame;
        roundTimer.value = 0;
        countdownTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(stateManager.currentGameState == GameState.PreGame && PlayersReady())
        {
            stateManager.currentGameState = GameState.PlayersReady;
            nextCountdownTick = Time.time + 1;
            gameStatusText.gameObject.SetActive(false);
            audioSource.PlayOneShot(countdownTick);
            countdownTimerText.text = countdownTimer + ""; //converting int to string with ""
        }
        else if (stateManager.currentGameState == GameState.PlayersReady)
        {
            if(Time.time > nextCountdownTick)
            {
                CountdownTick();
                nextCountdownTick = Time.time + 1;
            }
        }
        else if(stateManager.currentGameState == GameState.GameActive)
        {
            roundTimer.value = Utils.RoundToInt(Time.time - roundStartTime);

            //TODO Escalation Tiers
        }
        else if(stateManager.currentGameState == GameState.GameOver)
        {
            if(Input.GetKey(player1.bombKey) || Input.GetKey(player2.bombKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

         if(Input.GetKey("r"))
        {
            player1.playerScore.value = 0;
            player2.playerScore.value = 0;
        }

    }

    void CountdownTick()
    {
        countdownTimer--;

        if(countdownTimer == 0)
        {
            audioSource.PlayOneShot(countdownEnd);
            stateManager.currentGameState = GameState.GameActive;
            countdownTimerText.text = "";
            roundStartTime = Time.time;

            if(music)
            {
                audioSource.clip = music;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if(countdownTimer != 0)
        {
            {
                countdownTimerText.text = countdownTimer + "";
                audioSource.PlayOneShot(countdownTick);
            }

        }
    }

    public void OnPlayerKilled(PlayerBrain playerKilled)
    {
        
        if(playerKilled == player1)
        {
            Debug.Log("Player 2 Wins.");
            countdownTimerText.fontSize = 30;
            countdownTimerText.text = "Player 2 Wins! \n Hit Bomb Keys to Restart";
            player2.playerScore.value++;
        }
        else if(playerKilled == player2)
        {
            Debug.Log("Player 1 Wins.");
            countdownTimerText.fontSize = 30;
            countdownTimerText.text = "Player 1 Wins! \n Hit Bomb Keys to Restart";
            player1.playerScore.value++;
        }
        else
        {
            countdownTimerText.fontSize = 30;
            countdownTimerText.text = "Game Over! \n Hit Bomb Keys to Restart";
        }

        stateManager.currentGameState = GameState.GameOver;
        player1.ready = false;
        player2.ready = false;
        
    }

    bool PlayersReady()
    {
        if(player1.ready && !player2.playerGO)
        {
            return true;
        }
        if (player1.ready && player2.ready)
        {
            return true;
        }
        return false;
    }
}
