using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioManager asm; 
    [SerializeField] AudioClip LossSound;

    public static GameManager Instance { get; private set; }
    public enum GameState
    {
        GAME,
        PAUSE,
        DEFEAT,
        WIN
    }
    [SerializeField] private GameState currentState; 

    public event Action OnGameStateChanged;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }

    /*
    public int Score
    {
        get => score;
        set
        {
            score = value;

            if (OnScoreValueChanged != null)
                OnScoreValueChanged.Invoke(score); 
        }
    }
    private int score = 0;
    public UnityEvent<int> OnScoreValueChanged;
    */ 

    IEnumerator DelayedGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameOver();
    }

    void GameOver()
    {
        SwitchState(GameState.DEFEAT); 
        SceneManager.LoadScene("GameOver");
        asm.PlayOneShot(LossSound, false);  

        if (SceneManager.GetActiveScene().name == "GameOver")
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
                //Score = 3;
            }
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public void SwitchState(GameState newState)
    {
        Debug.Log("New state has been set to " + newState); 
        currentState = newState;
        OnGameStateChanged?.Invoke(); 
    }
}

