using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    
    public static event Action OnPlayerDeath;
    public static event Action<float> OnUpdatePosition;

    [SerializeField] private UIPopUpWindow gameOverWindow;


    public static float CurrentGameSpeed { get; private set; }
    
    
    public static Snake snake;
    public static SnakeInput snakeInput;
    public static FoodType currentFoodTypeSpawning;


    private bool isGameOver;
    private Coroutine restartGameCoroutine;

    private GameSettings gameSettings;

    private void Awake()
    {
        StopGame();
        
        if (!S)
            S = this;

        gameSettings = SettingsManager.S.gameSettings;
        
        Application.targetFrameRate = gameSettings.targetFrameRate;
        QualitySettings.vSyncCount = 0;

        CurrentGameSpeed = gameSettings.gameSpeed;
        
    }
    
    private void OnEnable()
    {
        SnakeBoostController.OnBoostStarted += OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded += OnBoostEndedHandler;
    }

    private void OnDisable()
    {
        SnakeBoostController.OnBoostStarted -= OnBoostStartedHandler;
        SnakeBoostController.OnBoostEnded -= OnBoostEndedHandler;
    }

    private void FixedUpdate()
    {
        OnUpdatePosition?.Invoke(-CurrentGameSpeed * Time.fixedDeltaTime);
    }

    

    #region On Snake Boost Handlers

    private void OnBoostStartedHandler()
    {
        StartCoroutine(SmoothlyUpdateCurrentGameSpeed(gameSettings.burstGameSpeed));
    }

    private void OnBoostEndedHandler()
    {
        StartCoroutine(SmoothlyUpdateCurrentGameSpeed(gameSettings.gameSpeed));
    }

    private IEnumerator SmoothlyUpdateCurrentGameSpeed(float targetGameSpeed)
    {
        while (!Mathf.Approximately(CurrentGameSpeed, targetGameSpeed))
        {
            CurrentGameSpeed = Mathf.MoveTowards(CurrentGameSpeed, targetGameSpeed, gameSettings.gameSpeedGain * Time
                .fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #region Game State Controls

    public void OnPlayerDeathHandler()
    {
        isGameOver = true;
        gameOverWindow.ShowWithDimmer();
        OnPlayerDeath?.Invoke();
        StartCoroutine(StopTime());
    }
    
    private IEnumerator StopTime()
    {
        var timer = 0f;
        while (timer <= gameSettings.timeToCompletelyStopTime)
        {
            var t = 1 - timer / gameSettings.timeToCompletelyStopTime;
            Time.timeScale = Mathf.Clamp01(t);
            yield return new WaitForFixedUpdate();
            timer += Time.unscaledDeltaTime;
        }
        Time.timeScale = 0;
    }
    
    public void StopGame()
    {
        Time.timeScale = 0;
        Snake.inputEnabled = false;
    }
    

    public void ResumeGame()
    {
        if (isGameOver)
        {
            gameOverWindow.ShowWithDimmer();
            return;
        }
        Time.timeScale = 1f;
        Snake.inputEnabled = true;
    }

    public void RestartGame()
    {
        if(restartGameCoroutine == null)
            restartGameCoroutine = StartCoroutine(RestartGameCoroutine());
    }
    
    private IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(gameSettings.restartDelay);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

}
