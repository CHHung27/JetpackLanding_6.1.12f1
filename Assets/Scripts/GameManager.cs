using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Lander Reference Notes
    // Q: We need a reference to the Lander
    // A:
    // [SerializeField] private Lander lander; // <- we can do this and drag lander object into the field in the inspector
    // OR use singleton pattern to always have a reference to the lander via the Lander class itself 
    #endregion

    private static int levelNumber = 1;  // static to persist between scene loading
    [SerializeField] private List<GameLevel> gameLevelsList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;


    private int score;
    private float time;
    private bool isTimerActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // getting the lander reference from the class itself since we initialized it as a singleton/static 
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;  
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    }


    /// <summary>
    /// interates through the game level list to find the correct level, spawns it, and then set the lander at correct starting position
    /// </summary>
    private void LoadCurrentLevel()
    {
        foreach (GameLevel gameLevel in gameLevelsList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                // spawn game level
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                // setting lander at start position
                Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
                // setting camera track target
                cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
                // setting camera zoom
                CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
            }
        }
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;  // isTimerActive is true if lander state is normal

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
    }

    private void Update()
    {
        // start keeping time only when timer is active -> lander is in normal state
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);  // add landing score
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    /// <summary>
    /// adds given parameter addScoreAmount to score
    /// </summary>
    /// <param name="addScoreAmount"></param>
    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log("Total Score: " + score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void GoToNextLevel()
    {
        levelNumber++;
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnPaused?.Invoke(this, EventArgs.Empty);
    }

    public void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }
}
