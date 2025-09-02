using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;


    private void Awake()
    {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.UnPauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenuScene);
        });
        musicVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicVolumeTextMesh.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();

        });
        soundVolumeButton.onClick?.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

        soundVolumeTextMesh.text = "SOUND " + SoundManager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = "MUSIC " + MusicManager.Instance.GetMusicVolume();
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        // show menu ui
        Show();
    }

    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        // hide menu ui
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
