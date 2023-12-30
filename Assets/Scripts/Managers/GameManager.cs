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
    [SerializeField] GameObject introDialogue;

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

    private void Start()
    {
        if (currentState == GameState.GAME)
        {
            if (introDialogue != null)
            {
                //Starting Dialogue
                introDialogue.SetActive(true);
                introDialogue.GetComponent<Dialogue>().StartDialogue();
            }
        }
    }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }
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

