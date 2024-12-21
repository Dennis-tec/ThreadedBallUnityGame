using UnityEngine;
using System.Threading;

/// <summary>
/// Controls the movement and behavior of the ball.
/// </summary>
public class BallMovement : MonoBehaviour
{
    /// <summary>
    /// Tag for player objects.
    /// </summary>
    public string PlayerTag = "Player";

    /// <summary>
    /// Indicates if the ball has collided with the player.
    /// </summary>
    public bool collided = false;

    /// <summary>
    /// Delay before destroying the ball after a collision.
    /// </summary>
    private float destructionDelay = 0.2f; // Delay before destroying the ball

    /// <summary>
    /// Rigidbody component of the ball.
    /// </summary>
    public Rigidbody2D rigidBody;

    /// <summary>
    /// Initializes the ball movement and sets the gravity scale.
    /// </summary>
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = BallConstants.GetGravityForLevel(); // Set Ball Gravity scale in Start
        Debug.Log(rigidBody.gravityScale);
    }

    /// <summary>
    /// Updates the ball's position and checks for destruction conditions.
    /// </summary>
    void Update()
    {
        // set gravity scale
        float currentYPosition = this.transform.position.y; // get current y co-ordinate\
        if (currentYPosition < BallConstants.minimumY || collided)
        {
            Destroy(this.gameObject, destructionDelay); // Destroy the ball with a slight delay
        }
    }

    /// <summary>
    /// Handles collision events with other game objects.
    /// </summary>
    /// <param name="collision">Collision information.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            collided = true;
        }
    }
}
