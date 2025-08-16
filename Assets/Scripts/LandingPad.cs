using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField]
    private int scoreMultiplier; // private so other classes cannot directly access it; instead, use the getter method defined below

    /// <summary>
    /// returns the private field scoreMultiplier
    /// </summary>
    /// <returns> scoreMultiplier </returns>
    public int getScoreMultiplier()
    {
        return scoreMultiplier;
    }
}