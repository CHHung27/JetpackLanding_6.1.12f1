using UnityEngine;
using UnityEngine.SceneManagement;

// static to make is persist and not related to any instance, but of the class itself
public static class SceneLoader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        GameOverScene,
    }

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
