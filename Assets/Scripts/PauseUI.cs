using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.UnPauseGame();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;

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
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
