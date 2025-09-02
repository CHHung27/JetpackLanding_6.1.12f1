using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private CanvasGroup settingsCanvasGroup;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button volumeButton;
    [SerializeField] private TextMeshProUGUI volumeTextMesh;


    private void Awake()
    {
        Time.timeScale = 1f;  // resets timeScale in case we got here from pause menu

        playButton.onClick.AddListener(() => {
            GameManager.ResetStaticData();  // resets level and totalScore variables
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        });

        settingsButton.onClick.AddListener(() => {
            settingsCanvasGroup.alpha = 1f;
            settingsCanvasGroup.interactable = true;
            settingsCanvasGroup.blocksRaycasts = true;
            volumeTextMesh.text = "VOLUME " + MusicManager.Instance.GetMusicVolume();
            HideMenuButtons();
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        volumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            volumeTextMesh.text = "VOLUME " + MusicManager.Instance.GetMusicVolume();
        });

        BackButton.onClick.AddListener(() => {
            settingsCanvasGroup.alpha = 0f;
            settingsCanvasGroup.interactable = false;
            settingsCanvasGroup.blocksRaycasts = false;
            ShowMenuButtons();
        });
    }

    private void Start()
    {
        playButton.Select();  // selects the play button for gamepad users
    }

    private void ShowMenuButtons()
    {
        playButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    private void HideMenuButtons()
    {
        playButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }
}
