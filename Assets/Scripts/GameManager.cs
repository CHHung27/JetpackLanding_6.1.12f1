using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // singleton pattern

    private static int levelNumber = 1; // keeps levelnumber; static to persist between scene loading
    private static int totalScore = 0;  // keeps total score; static to persist between scene loading

    #region Lander Reference Notes
    // Q: We need a reference to the Lander
    // A: [SerializeField] private Lander lander; // <- we can do this and drag lander object into the field in the inspector
    //    OR use singleton pattern to always have a reference to the lander via the Lander class itself (what we usually end up doing)
    #endregion
    [SerializeField] private List<GameLevel> gameLevelsList;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    private int score;
    private float time;
    private bool isTimerActive;


    private void Awake()
    {
        Instance = this;  // singleton design pattern
    }

    private void Start()
    {
        // event listener attached to the lander reference from the class itself since we initialized it as a singleton/static 
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;  
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        // event listener attached to OnMenuButtonPressed event
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
        LoadCurrentLevel();
    }

    #region Lander Events
    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;  // isTimerActive is true if lander state is normal

        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
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
    #endregion

    #region Game Input Events
    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    } 
    #endregion

    private void Update()
    {
        // start keeping time only when timer is active -> lander is in normal state
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }
    
    #region Level Loading/Progressing
    /// <summary>
    /// interates through the game level list to find the correct level, spawns it, and then set the lander at correct starting position
    /// </summary>
    private void LoadCurrentLevel()
    {
        GameLevel gameLevel = GetGameLevel();
        // spawn game level
        GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        // setting lander at start position
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        // setting camera track target
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        // setting camera zoom
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }

    /// <summary>
    /// returns the game level if level number is valid, null otherwise (null meaning there's no next level)
    /// </summary>
    /// <returns></returns>
    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelsList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    /// <summary>
    /// increments level number and score, then uses GetGameLevel() to proceed to next level
    /// if GetGameLevel() returns null, then go to GameOverScene
    /// </summary>
    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;

        if (GetGameLevel() == null)
        {
            // no more levels, go to ending scene
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
        }
        else
        {
            // more levels to go
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }

    } 
    #endregion
    
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

    public void RetryLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    #region Pause/Unpause
    /// <summary>
    /// pauses the game by setting the time scale to zero; invokes OnGamePaused event
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// unpauses the game by setting the time scale to one; invokes OnGameUnPaused event
    /// </summary>
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        OnGameUnPaused?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// pause or unpauses the game according to its current state
    /// </summary>
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
    #endregion

    /// <summary>
    /// Reset static data to start of game status
    /// </summary>
    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }
}
