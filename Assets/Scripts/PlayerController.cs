using UnityEngine;
using TMPro;
using System.Threading;

/// <summary>
/// Controls the player character and handles interactions with game objects.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Player's score.
    /// </summary>
    public int score;  // initiliaze the players score

    // Game Tags
    /// <summary>
    /// Tag for ball objects.
    /// </summary>
    public string BallTag;

    /// <summary>
    /// Tag for two points coin objects.
    /// </summary>
    public string TwoPointsCoin;

    /// <summary>
    /// Tag for three points coin objects.
    /// </summary>
    public string ThreePointsCoin;

    // Texts to update
    /// <summary>
    /// UI text for displaying the current score.
    /// </summary>
    public TextMeshProUGUI currentScore;

    /// <summary>
    /// UI text for displaying the high score.
    /// </summary>
    public TextMeshProUGUI highScore;

    /// <summary>
    /// UI text for displaying the high score label.
    /// </summary>
    public TextMeshProUGUI HighScoreText;

    // Sounds properties
    /// <summary>
    /// Audio source for ball hit sound.
    /// </summary>
    public AudioSource ballHitSound;

    /// <summary>
    /// Audio source for gaining coin sound.
    /// </summary>
    public AudioSource gainCoinSound;

    /// <summary>
    /// Audio source for destroying coins sound.
    /// </summary>
    public AudioSource destroyCoinsSound;

    // Game Objects
    /// <summary>
    /// Prefab for the two points coin.
    /// </summary>
    public GameObject twoPointsCoin; // Assign the two points coin prefab in the inspector

    /// <summary>
    /// Prefab for the three points coin.
    /// </summary>
    public GameObject threePointsCoin; // Assign the three points coin prefab in the inspector

    /// <summary>
    /// Instance of the SqlDatabase class for database interactions.
    /// </summary>
    private SqlDatabase sqlDatabaseInstance;

    /// <summary>
    /// Initializes the player controller and sets up initial parameters.
    /// </summary>
    public void Start()
    {
        sqlDatabaseInstance = new SqlDatabase();
        sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.GetHighestScoreOperation);
        score = 0;
        highScore.text = SqlDatabaseConstants.RecordedHighScore.ToString("0000");
        ExecuteCoinThreadsOnUnityMainThread.Instance();
    }

    /// <summary>
    /// Updates the player movement.
    /// </summary>
    public void Update()
    {
        movePlayer();
    }

    /// <summary>
    /// Adjusts the player's position based on the arrow keys while ensuring that the next position is within the game scene limits.
    /// </summary>
    private void movePlayer()
    {
        Vector3 moveToPosition = Vector3.zero;   // initilize 2D position to zero for proper addition
        float verticalPosition = transform.position.y; // get current y co-ordinate
        float horizontalPosition = transform.position.x; // get current x co-ordinate

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // don't move the player above the top limit
            if (verticalPosition < PlayerConstants.UpperLimit)
            {
                moveToPosition += Vector3.up;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // don't move the player below the bottom limit
            if (verticalPosition > PlayerConstants.BottomLimit)
            {
                moveToPosition += Vector3.down;
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // don't move the player beyond the left limit
            if (horizontalPosition > PlayerConstants.LeftLimit)
            {
                moveToPosition += Vector3.left;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            // don't move the player beyond the right limit
            if (horizontalPosition < PlayerConstants.RightLimit)
            {
                moveToPosition += Vector3.right;
            }
        }
        // apply the user keyboard arrow adjustment to move the player to the required position
        transform.position += moveToPosition * PlayerConstants.MoveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Handles collision events with other game objects.
    /// </summary>
    /// <param name="collision">Collision information.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(BallTag))
        {
            ballHitSound.Play();
            IncreamentScore(5);
            float yPos = transform.position.y;
            Thread coinThread = new Thread(() => CreateCoinOnTheMainThread(twoPointsCoin));
            Thread coinThread2 = new Thread(() => CreateCoinOnTheMainThread(threePointsCoin));
            if (yPos >= -2.5 && yPos < 1)
            {
                coinThread.Start();
            }
            else if (yPos >= 1)
            {
                coinThread2.Start();
            }
        }

        if (collision.gameObject.CompareTag(TwoPointsCoin))
        {
            destroyCoinsSound.Play();
            IncreamentScore(2);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag(ThreePointsCoin))
        {
            destroyCoinsSound.Play();
            IncreamentScore(3);
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Increments the player's score and updates the UI.
    /// </summary>
    /// <param name="addVal">Value to add to the score.</param>
    private void IncreamentScore(int addVal)
    {
        score += addVal;
        currentScore.text = score.ToString("0000");
        if (score > SqlDatabaseConstants.RecordedHighScore)
        {
            newHighScore();
            HighScoreText.text = "New Score:";
        }
        BallsGenerator.AttainedScore = score;
    }

    /// <summary>
    /// Updates the high score UI and records the new high score.
    /// </summary>
    private void newHighScore()
    {
        highScore.text = score.ToString("0000");
        SqlDatabaseConstants.RecordedHighScore = score;
    }

    /// <summary>
    /// Adds the function to create a coin to the main Unity thread.
    /// </summary>
    /// <param name="coinToCreate">The coin prefab to create.</param>
    private void CreateCoinOnTheMainThread(GameObject coinToCreate)
    {
        ExecuteCoinThreadsOnUnityMainThread.Instance().Enqueue(() => InstantiateCoin(coinObject: coinToCreate));
    }

    /// <summary>
    /// Instantiates a coin at a random position within the game scene.
    /// </summary>
    /// <param name="coinObject">The coin prefab to instantiate.</param>
    private void InstantiateCoin(GameObject coinObject)
    {
        float yPos = Random.Range(-4.5f, 3f);
        float xPos = Random.Range(-4.5f, 4.5f);
        GameObject newCoin = Instantiate(coinObject, new Vector3(xPos, yPos, 0f), Quaternion.identity);
        gainCoinSound.Play();
    }
}
