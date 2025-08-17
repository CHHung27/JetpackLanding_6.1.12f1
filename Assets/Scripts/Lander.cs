using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// to use this script (and any script for that matter) as an component, we need to inherit from MonoBehaviour
public class Lander : MonoBehaviour
{
    /*
     * Singleton pattern: creating an instance of the class itself. Essentially an easy way to 
     * create an global reference to an uniquely exisiting object (only one of this instance exists).
     * Other classes can get a reference to this instance by calling the class.
     * 
     * Static: belonging to the class itself, not any instances
     */
    public static Lander Instance { get; private set; }  // also remember to set reference to self in Awake()


    #region Events
    // EventHandlers call events, which notifies other classes when something happens
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce; 
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;  // <OnLandedEventArgs> is a generic label
    public class OnLandedEventArgs : EventArgs
    {
        public int score;
    }  // custom eventArgs class to pass additional information along with the invoked event
    #endregion

    #region Fields
    // fields should almost always be private
    private Rigidbody2D landerRigidbody2D;  // stores our lander's Rigidbody2D
    private float fuelAmount = 10f; 
    #endregion

    private void Awake()
    {
        // general rule of thumb: grab local references in Awake() and external references in Start()
        Instance = this;  // initializing the class instance
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate() runs on a fixed timestep (see project settings) independent of framerate. Useful to ensure consistent physics performance
    // Note: since this doesn't run every frame, unline Update(), we might miss key inputs such as keyDown()
    // Key holds should be fine, key downs might be missed
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);  // constantly invoke OnBeforeForce event, which turns off thruster emissions in LanderVisuals.cs

        // if no fuel, don't receive input
        if (fuelAmount <= 0)
        {
            return;
        }

        #region Player Input
        // if any key is pressed, consumeFuel()
        if (Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            ConsumeFuel();
        }

        // getting current active keyboard input using input system (new)
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;                                                 // use local variables with clear descriptive names, not magic numbers!
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);  // using transform.up allows the force to be applied where the lander is pointing, rather the global "up"
            OnUpForce?.Invoke(this, EventArgs.Empty);         // (?) is the null conditional operator. Operation continues only if OnUpForce is not null
                                                              // .Invoke(object invoking event, args to send with event); Invoked events can be listened
                                                              // to by other classes
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);  // Time.deltaTime helps with mitigating client framerate differences as it splits up the
                                                                      // physics calculations relative to the time elapsed between frames
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        } 
        #endregion
    }

    // using 2D version of OnCollisionEnter since 3D version won't work properly
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // crash if not colliding with landing pad
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on terrain!");
            return;
        }

        float softLandingVelocityMagnitude = 4f;                                   // threshold for "soft" landing
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;  // stores relative velocity/intensity of collision
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            // landed too hard
            Debug.Log("Landing too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);                   // using dot product to check angle relative to up vector
        float minDotVector = 0.90f;
        if (dotVector < minDotVector)
        {
            // landed on a steep angle
            Debug.Log("landed on a too steep angle!");
            return;
        }

        Debug.Log("Successful Landing!");

        #region Landing Score Calculation
        float maxScoreAmountLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.getScoreMultiplier());
        Debug.Log("Score: " + score);

        OnLanded?.Invoke(this, new OnLandedEventArgs {
            score = score
        });  // custom eventArgs for when we want to pass on additional information with our invoked event (i.e. score)
        #endregion
    }

    /// <summary>
    /// runs just once when hitting a trigger2D
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        // refuel and destroy fuel pickup on trigger
        // reminder: we made a script for fuelPickup so we can identity via component class
        if (collider2D.gameObject.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;    // reminder: NO MAGIC NUMBERS
            fuelAmount += addFuelAmount;
            fuelPickup.destroySelf();
        }

        if (collider2D.gameObject.TryGetComponent<CoinPickup>(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);  // invoke OnCoinPickup event; score managed by GameManager
            coinPickup.DestroySelf();
        }
    }
        

    // Update is called once per frame (EVERY SINGLE FRAME)
    // we explicitly define this function as private to uphold clear, high quality code standards
    private void Update()
    {

    }


    /// <summary>
    /// Takes away fuelConsumptionAmount from fuel, adjusted for Time.deltaTime (framerate differences)
    /// </summary>
    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }
}
