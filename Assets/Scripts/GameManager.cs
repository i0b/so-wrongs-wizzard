using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameEnded;


    public static GameManager Instance;

    enum GameState
    {
        None,
        Countdown,
        Started,
        EnterHighscore,
        GameOver,
        Highscore,
        Help
    }

    int score = 0;
    bool gameOver = false;

    public bool GameOver { get { return gameOver; } }

    private void Awake()
    {
        Instance = this;
    }

    void SetGameState(GameState state)
    {
        switch(state)
        {
            case GameState.None:
                break;

            case GameState.Countdown:
                break;

            case GameState.Started:
                break;

            case GameState.EnterHighscore:
                break;

            case GameState.GameOver:
                break;

            case GameState.Highscore:
                break;

            case GameState.Help:
                break;
        }
    }

    public void ConfirmGameOver() {
        OnGameEnded();
    }

    public void StartGame() {
        OnGameStarted();
    }
}
