using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private Vector3 _menuPos = new Vector3(-9940f, -9998.43f, -9955f);
    private Quaternion _menuRotation = new Quaternion(0, -0.47f, 0, 0.88f);
    private Vector3 _startPos = new Vector3(26f, 30f, 6f);
    private Quaternion _startRotation = new Quaternion(0, 0.71f, 0, 0.71f);

    public enum GameState
    {
        Menu,
        Gameplay,
        EndGame
    }

    private GameState _currentGameState;
    public GameState CurrentGameState { get { return _currentGameState; } }

    //Singleton
    public static GameStateManager I_GameStateManager { get; set; }
    private void Awake()
    {
        if (I_GameStateManager == null)
        {
            I_GameStateManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //_currentGameState = GameState.Menu;

        //For testing only
        Menu();
    }

    public void Menu()
    {
        _currentGameState = GameState.Menu;
        _player.GetComponent<CarController2>().enabled = false;
        CameraManager.I_CameraManager.SwitchCameraToMenu();
        GameUIManager.I_GameUIManager.GameplaySwitch(false);
        GameUIManager.I_GameUIManager.EndCardOff();

        //Set player position
        _player.transform.position = _menuPos;
        _player.transform.rotation = _menuRotation;
    }

    public void PlayGame()
    {
        _currentGameState = GameState.Gameplay;
        _player.GetComponent<CarController2>().enabled = true;
        CameraManager.I_CameraManager.SwitchCameraToPlayer();
        GameUIManager.I_GameUIManager.GameplaySwitch(true);
        GameUIManager.I_GameUIManager.EndCardOff();

        //Set player position
        _player.GetComponent<CarController2>().ResetPosition();
        _player.transform.position = _startPos;
        _player.transform.rotation = _startRotation;

        //RESET everything
        GameTimer.I_GameTimer.ResetTimer();
        QuestManager.I_QuestManager.ResetQuests();
        MoneyManager.I_MoneyManager.ResetMoney();
        EndCardStats.I_EndCardStats.ResetEndStats();
    }

    public void EndGame()
    {
        _currentGameState = GameState.EndGame;
        Time.timeScale = 1;
        _player.GetComponent<CarController2>().enabled = false;
        GameUIManager.I_GameUIManager.EndGame();
        GameTimer.I_GameTimer.PauseTimer();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
