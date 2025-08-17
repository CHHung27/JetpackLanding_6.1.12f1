using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    [SerializeField] private GameObject landerExplosionVFX;
    

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();

        // attaching listener functions onto events
        lander.OnUpForce += Lander_OnUpForce;        
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        // setting thrusters to off
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        
        switch (e.landingType)
        {
            case Lander.LandingType.WrongLandingArea:
            case Lander.LandingType.TooSteepAngle:
            case Lander.LandingType.TooFastLanding:
                // Crash: show explosionVFX
                Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// OnBeforeForce listener function. Executes when OnBeforeForce is invoked.
    /// Turns off all ParticleSystem emissions 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        // setting thrusters to off before any input is received
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, false);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, false);
    }


    /// <summary>
    /// OnRightForce listener function. Executes when OnRightForce event is invoked
    /// Turns off all ParticleSystem emissions except for the left one
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        // enable left thruster when heading right
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
    }


    /// <summary>
    /// OnLeftForce listener function. Executes when OnLeftForce event is invoked
    /// Turns off all ParticleSystem emissions except for the right one
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        // enable right thruster when heading left
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }


    /// <summary>
    /// OnUpForce listener function. Executes when OnUpForce event is invoked
    /// Turns on all ParticleSystem emissions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        // enable all particle emissions when up force event called
        SetEnabledThrusterParticleSystem(leftThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(middleThrusterParticleSystem, true);
        SetEnabledThrusterParticleSystem(rightThrusterParticleSystem, true);
    }


    /// <summary>
    /// Enables/Disables emission module of given particleSystem
    /// </summary>
    /// <param name="particleSystem"></param>
    /// <param name="enabled"></param>
    private void SetEnabledThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.enabled = enabled;
    }
}
