using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStateManager;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _minimapPanel;

    [SerializeField] private QuestComplete _questComplete;
    [SerializeField] private GameObject _endCardPanel;

    [SerializeField] private GameObject _ingamePanel;

    //Singleton
    public static GameUIManager I_GameUIManager { get; set; }
    private void Awake()
    {
        if (I_GameUIManager == null)
        {
            I_GameUIManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (I_GameStateManager.CurrentGameState == GameState.Gameplay)
        {
            if (_pausePanel.activeSelf == true || _questPanel.activeSelf == true || _mapPanel.activeSelf == true)
            {
                GameTimer.I_GameTimer.PauseTimer();
                Time.timeScale = 0;
            }
            else
            {
                GameTimer.I_GameTimer.ResumeTimer();
                Time.timeScale = 1;
            }
        }
    }

    public void PauseSwitch()
    {
        if (I_GameStateManager.CurrentGameState != GameState.Gameplay)
        {
            return;
        }
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        _questPanel.SetActive(false);
        _mapPanel.SetActive(false);
        _minimapPanel.SetActive(true);
    }

    public void QuestSwitch()
    {
        if (I_GameStateManager.CurrentGameState != GameState.Gameplay)
        {
            return;
        }
        _pausePanel.SetActive(false);
        _questPanel.SetActive(!_questPanel.activeSelf);
        _mapPanel.SetActive(false);
        _minimapPanel.SetActive(!_questPanel.activeSelf);
    }

    public void MapSwitch()
    {
        if (I_GameStateManager.CurrentGameState != GameState.Gameplay)
        {
            return;
        }
        _pausePanel.SetActive(false);
        _questPanel.SetActive(false);
        _mapPanel.SetActive(!_mapPanel.activeSelf);
        _minimapPanel.SetActive(!_mapPanel.activeSelf);
    }

    public void QuestComplete()
    {
        _questComplete.QuestCompleted();
    }

    public void EndGame()
    {
        _endCardPanel.SetActive(true);
        _endCardPanel.GetComponent<EndCard>().GameCompleted();
        EndCardStats.I_EndCardStats.UpdateStats();
        _pausePanel.SetActive(false);
        _questPanel.SetActive(false);
        _mapPanel.SetActive(false);
        _minimapPanel.SetActive(false);
    }

    public void EndCardOff()
    {
        _endCardPanel.SetActive(false);
    }

    public void GameplaySwitch(bool isOn)
    {
        _menuPanel.SetActive(!isOn);
        _ingamePanel.SetActive(isOn);
        _minimapPanel.SetActive(isOn);
    }
}
