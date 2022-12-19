using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _instructionPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _lifeText;
    [SerializeField] private TextMeshProUGUI _scoreTextInLoseMenu;
    [SerializeField] private TextMeshProUGUI _scoreTextInWinMenu;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private GameObject[] _levelLabels;
    [SerializeField] private Texture2D targetTexture;
    [SerializeField] private Texture2D cursorTexture;
    public bool optionsFromMainMenu = true;
    private static PopupManager instance = null;

    public static PopupManager Instance
    {
        get { return instance; }
    }


    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        GameManager.OnGameStateChanged += GameManagerOnStateChaged;
    }

  
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChaged;
    }


    private void GameManagerOnStateChaged(GameState state)
    {
        _menuPanel.SetActive(state == GameState.MainMenu);
        _gamePanel.SetActive(state == GameState.GameIsWorking);
        _pausePanel.SetActive(state == GameState.PauseMenu);
        _winPanel.SetActive(state == GameState.WinGame);
        _losePanel.SetActive(state == GameState.LoseGame);
        _instructionPanel.SetActive(state == GameState.Instructions);
        _optionsPanel.SetActive(state == GameState.Options);
        if (state == GameState.GameIsWorking)
        {
            Cursor.SetCursor(targetTexture, new Vector2(targetTexture.width / 2, targetTexture.height / 2), CursorMode.ForceSoftware);       
        }
        else
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    public void StartGameButton()
    {
        GameManager.Instance.UpdateGameState(GameState.GameIsWorking);
    }

    public void OptionButton()
    {
        GameManager.Instance.UpdateGameState(GameState.Options);
    }
    public void InstructionButton()
    {
        GameManager.Instance.UpdateGameState(GameState.Instructions);
    }
    public void ExitButton()
    {
        GameManager.Instance.SaveDate();
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void BackToMainMenuButton()
    {
        if (optionsFromMainMenu)
        {
            GameManager.Instance.UpdateGameState(GameState.MainMenu);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.PauseMenu);
        }
    }

    public void ReloadScene()
    {
        GameManager.Instance.SaveDate();
        SceneManager.LoadScene(0);
    }

    public void ChangeScoreText()
    {
        if (GameManager.Instance.Level < 5)
        {
            _scoreText.text = "Goal " + GameManager.Instance.Score + "/" + (GameManager.Instance.Level * 10);
        }
        else
        {
            _scoreText.text = "Score " + GameManager.Instance.Score;
        }
    }

    public void ChangeLifeText()
    {
        _lifeText.text = "Life:" + GameManager.Instance.life;
    }
    public void LevelLabel(bool state)
    {
        if (GameManager.Instance.Level >=2)
        {
            int level = GameManager.Instance.Level - 2;
            _levelLabels[level].SetActive(state);
        }
    }

    public void ScoreInLoseMenu()
    {
        _scoreTextInLoseMenu.text = "Your score:" + GameManager.Instance.Score;
    }
    public void ScoreInWinMenu()
    {
        _scoreTextInWinMenu.text = "Your score:" + GameManager.Instance.Score;
    }


    public void BestScoreText()
    {
        _bestScore.text = "Best score:" + GameManager.Instance.bestScore;
    }
}
