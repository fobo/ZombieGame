using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Inventory,
    LevelSelect,
    DeathMenu
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {

        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    //return the current state of the game to check for things like "I SHOULD NOT FIRE MY GUN IF THE GAME IS PAUSED" stupid rockets
    public GameState GetGameState(){
        return CurrentState;
    }

    //fast way to check if the game is being played or not (not paused)
    public bool IsGamePlaying(){
        if(CurrentState == GameState.Playing) return true;

        return false;
    }
    public void SetState(GameState newState)
    {
        CurrentState = newState;

        // Automatically manage time scale
        Time.timeScale = (newState == GameState.Playing) ? 1 : 0;
    }
}
