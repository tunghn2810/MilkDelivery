using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using static GameStateManager;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private GameObject _clock;

    private bool _isPaused = false;

    private const float INITIAL_TIME = 180f;
    private float _timeRemaining = INITIAL_TIME;
    private float _totalTime = 0;
    public float TotalTime { get => _totalTime; }

    private bool _gameEnded = true;

    //Singleton
    public static GameTimer I_GameTimer { get; set; }
    private void Awake()
    {
        if (I_GameTimer == null)
        {
            I_GameTimer = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _totalTime += Time.deltaTime;
        if (_timeRemaining > 0f)
        {
            if (!_isPaused)
                _timeRemaining -= Time.deltaTime;
        }
        else
        {
            _timeRemaining = 0f;
            if (_gameEnded)
            {
                I_GameStateManager.EndGame();
                _gameEnded = false;
            }
        }
        DisplayTime();
    }

    private void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(_timeRemaining / 60f);
        float seconds = Mathf.FloorToInt(_timeRemaining % 60f);
        float ticks = Mathf.FloorToInt((_timeRemaining * 100f) % 100f);

        _clock.GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, ticks);
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }

    public void IncreaseTimer(float amount)
    {
        _timeRemaining += amount;
        EndCardStats.I_EndCardStats.AddTime(amount);
    }

    public void ResetTimer()
    {
        _isPaused = false;
        _timeRemaining = INITIAL_TIME;
        _totalTime = 0;
        _gameEnded = true;
    }
}
