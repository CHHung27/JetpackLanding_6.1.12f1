using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Time.timeScale = 1f;  // resets timeScale in case we got here from pause menu

        playButton.onClick.AddListener(() => {
            GameManager.ResetStaticData();  // resets level and totalScore variables
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start()
    {
        playButton.Select();  // selects the play button for gamepad users
    }
}
