using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] 
    private TextMeshPro scoreMultiplierTextMesh;  // TextMeshPro component attached to the LandingPad

    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierTextMesh.text = "x" + landingPad.GetScoreMultiplier();        // sets up the landing pad TextMeshPro text to say xscoreMultiplier. This is a modular way of setting up landingPad multiplier text asset
    }
}
