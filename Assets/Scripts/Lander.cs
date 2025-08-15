using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// to use this script as an component, we need to inherit from MonoBehaviour
public class Lander : MonoBehaviour
{
    // fields should almost always be private
    private Rigidbody2D landerRigidbody2D;  // stores our lander's Rigidbody2D

    private void Awake()
    {
        // general rule of thumb: grab local references in Awake() and external references in Start()
        landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate() runs on a fixed timestep (see project settings) independent of framerate. Useful to ensure consistent physics performance
    // Note: since this doesn't run every frame, unline Update(), we might miss key inputs such as keyDown()
    // Key holds should be fine, key downs might be missed
    private void FixedUpdate()
    {
        // getting current active keyboard input using input system (new)
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;  // use local variables with clear descriptive names, not magic numbers!
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);  // using transform.up allows the force to be applied where the lander is pointing, rather the global "up"
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            float turnSpeed = +100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);  // Time.deltaTime helps with mitigating client framerate differences as it splits up the physics calculations relative to the time elapsed between frames
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    // using 2D version of OnCollisionEnter since 3D version won't work properly
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on terrain!");
            return;
        }

        float softLandingVelocityMagnitude = 4f;
        if (collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            // landed too hard
            Debug.Log("Landing too hard!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = 0.90f;
        if (dotVector < minDotVector)
        {
            // landed on a steep angle
            Debug.Log("landed on a too steep angle!");
            return;
        }

        Debug.Log("Good Landing!");
    }

    // Update is called once per frame (EVERY SINGLE FRAME)
    // we explicitly define this function as private to uphold clear, high quality code standards
    private void Update()
    {
        
        
    }
}
