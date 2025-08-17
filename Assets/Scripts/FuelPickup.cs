using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    /// <summary>
    /// removes self game object
    /// </summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
