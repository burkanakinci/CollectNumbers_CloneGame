using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    #region Fields
    public JsonConverter JsonConverter;
    public PlayerManager PlayerManager;
    public Entities Entities;
    public ObjectPool ObjectPool;
    public LevelManager LevelManager;
    public InputManager InputManager;
    public UIManager UIManager;
    public CameraManager CameraManager;
    public GridManager GridManager;
    #endregion
    #region Actions
    public event Action OnResetToMainMenu;
    public event Action OnGameStart;
    public event Action OnLevelSuccess;
    public event Action OnLevelFailed;
    #endregion

    public void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        PlayerPrefs.DeleteAll();

        InitializeGameManager();
    }
    private void Start()
    {
        ResetToMainMenu();
    }

    public void InitializeGameManager()
    {
        Entities.Initialize();
        JsonConverter.Initialize();
        PlayerManager.Initialize();
        InputManager.Initialize();
        UIManager.Initialize();
        ObjectPool.Initialize();
        LevelManager.Initialize();
        GridManager.Initialize();
        CameraManager.Initialize();
    }

    #region Events
    public void ResetToMainMenu()
    {
        OnResetToMainMenu?.Invoke();
    }
    public void GameStart()
    {
        OnGameStart?.Invoke();
    }
    public void LevelSuccess()
    {
        OnLevelSuccess?.Invoke();
    }
    public void LevelFailed()
    {
        OnLevelFailed?.Invoke();
    }
    #endregion
}