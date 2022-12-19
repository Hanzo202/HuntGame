using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;
    private GameState state;

    public GameState State
    {
        get { return state; }
    }
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            return instance; 
        }
    }
    private int level;

    public int Level
    {
        get { return level; }
    }
    private int score;
    public int Score
    {
        get { return score; }
    }
    
   
    private bool isLevelLabel = false;
    internal float life;    
    internal int bestScore;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] spawns;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.MainMenu);              
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && State == GameState.GameIsWorking)
        {
            UpdateGameState(GameState.PauseMenu);
        }

        if (isLevelLabel && Input.GetKeyDown(KeyCode.Space))
        {
            UpdateGameState(GameState.GameIsWorking);
            TargetsScript[] targetsInScene = FindObjectsOfType<TargetsScript>();
            foreach (TargetsScript target in targetsInScene)
            {
                Debug.Log("Destroy" + target.name);
                Destroy(target);
            }
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                LoadDate();
                PopupManager.Instance.optionsFromMainMenu = true;
                level = 1;
                life = 10;
                PopupManager.Instance.BestScoreText();
                break;
            case GameState.GameIsWorking:
                InGame();
                break;
            case GameState.NextLevel:
                LoadNextLevel();
                break;
            case GameState.PauseMenu:
                Time.timeScale = 0;
                PopupManager.Instance.optionsFromMainMenu = false;
                break;
            case GameState.LoseGame:
                AudioManager.Instance.GameOverSound();
                PopupManager.Instance.ScoreInLoseMenu();
                Time.timeScale = 0;
                break;
            case GameState.WinGame:
                AudioManager.Instance.NextLevelOrWinSound();
                PopupManager.Instance.ScoreInWinMenu();
                Time.timeScale = 0;
                break;
            case GameState.Options:
                break;
            case GameState.Instructions:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void InGame()
    {
        Time.timeScale = 1;
        AudioManager.Instance.StartGameSound();
        player.SetActive(true);
        foreach (GameObject spawn in spawns)
        {
            spawn.SetActive(true);
        }
        Time.timeScale = 1;
        PopupManager.Instance.LevelLabel(false);
        PopupManager.Instance.ChangeLifeText();
        PopupManager.Instance.ChangeScoreText();
    }

    public void LoadNextLevel()
    {
        AudioManager.Instance.NextLevelOrWinSound();
        Time.timeScale = 0;
        isLevelLabel = true;
        level++;
        PopupManager.Instance.LevelLabel(true);
    }


    public void TakePoint()
    {
        score++;
        PopupManager.Instance.ChangeScoreText();
        if (score == (level * 10) && level != 5)
        {
            Debug.Log(level);
            UpdateGameState(GameState.NextLevel);
        }
    }

    public void LoseOrWinMenu()
    {
        if (level != 5)
        {
            UpdateGameState(GameState.LoseGame);
        }
        else
        {
            UpdateGameState(GameState.WinGame);
        }
    }

    public void SaveDate()
    {
        if (score > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    public void LoadDate()
    {
        bestScore = PlayerPrefs.GetInt("BestScore");
    }
}

public enum GameState
{
    MainMenu,
    GameIsWorking,
    NextLevel,
    PauseMenu,
    LoseGame,
    WinGame,
    Options,
    Instructions,
}
