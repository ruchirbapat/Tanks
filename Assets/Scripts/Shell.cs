// Dependencies
using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    //Reference to the Rigidbody component attached to each Shell
    public Rigidbody attachedRigidbody;

    // Minimum force with which the Shell can be ejected (value is set in the Unity Editor)
    public float forceMin;

    // Maximum force with which the Shell can be ejected (value is set in the Unity Editor)
    public float forceMax;

    // Shell's lifetime i.e. amount of time before it gets destroyed
    public float lifetime = 4;

    // How long the Shell takes to fade away (seconds)
    public float fadetime = 2;

    // Unity's Start function that is called once per GameObject per Scene
    void Start()
    {
        // Using Random Number Generation, generate a random amount of force with which the bullet can be shot
        float force = Random.Range(forceMin, forceMax);

        // Apply a force to the Shell in the RIGHT direction
        attachedRigidbody.AddForce(transform.right * force);

        // Add some torque (rotational force) to the Shell to make it's ejection more realistic
        attachedRigidbody.AddTorque(Random.insideUnitSphere * force);

        // Once the Shell has been created start the Fade coroutine to animate its disappearing...
        StartCoroutine(Fade());
    }

    // This ALGORITHM is custom coroutine which animates the Shell fading away. It works by linearly interpolating 
    // on the Shell's Material's colour
    IEnumerator Fade()
    {
        // Wait for the 'lifetime' before beginning to fade (and destroy) the Shell
        yield return new WaitForSeconds(lifetime);

        // Stores how complete the fade Animation is i.e. its progress
        float percent = 0; // Values will range from 0.0f to 1.0f

        // Calculate the fade speed using Speed = Distance / Time
        float fadeSpeed = 1 / fadetime;

        // Get a reference to the Shell's attached Material
        Material mat = GetComponent<Renderer>().material;

        // Get the Material's colour
        Color initialCol = mat.color;

        // Iterate until the animation has completed
        while (percent < 1) {
            
            // Increase percentage by which the animation has completed since the last frame i.e. Time.deltaTime * fadeSpeed
            percent += Time.deltaTime * fadeSpeed;

            // Change the Shell's colour to 'percent' between it's initial colour and a clear colour (alpha = 0)
            mat.color = Color.Lerp(initialCol, Color.clear, percent);

            // Continue the while loop at the start of the next frame (simulates multithreading)
            yield return null;
        }

        // Animation has completed... destroy the Shell GameObject from the World
        Destroy(gameObject);
    }
}