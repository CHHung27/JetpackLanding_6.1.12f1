using UnityEngine.SceneManagement;

// static to make is persist and not related to any instance, but of the class itself
public static class SceneLoader
{
    /// <summary>
    /// Enum of the scenes in our game. Order and name should exactly match scenes in Build Profiles -> Scene List
    /// </summary>
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        GameOverScene,
    }

    /// <summary>
    /// loads the specified scene using Unity's SceneManager
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadScene(Scene scene)
    {
        // uses the scene string name (be very careful with how we name our scenes; they have to match the Scene enum exactly)
        SceneManager.LoadScene(scene.ToString());
    }
}
