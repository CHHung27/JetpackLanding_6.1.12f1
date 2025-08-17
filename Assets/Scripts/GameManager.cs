using UnityEngine;

public class GameManager : MonoBehaviour
{
    // need reference to the Lander
    //[SerializeField] private Lander lander; // 1) can do this and drag lander object in the, OR do what we did below
    private int score;

    private void Start()
    {
        // getting the lander reference from the class itself since we initialized it as a singleton/static 
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;  
        Lander.Instance.OnLanded += Lander_OnLanded;          
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
}
