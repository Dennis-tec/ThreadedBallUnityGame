using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.SceneManagement;
/// <summary>
/// Generates balls in the game and manages their creation using multiple threads.
/// </summary>
public class BallsGenerator : MonoBehaviour
{
    /// <summary>
    /// Prefab of the ball to be instantiated.
    /// </summary>
    public GameObject ballPrefab; // Assign the ball prefab in the inspector

    /// <summary>
    /// Indicates if a collision has occurred.
    /// </summary>
    public bool collided = false;

    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// Semaphore for synchronizing thread access.
    /// </summary>
    private static Semaphore semaphore = new Semaphore(1, 1); 

    /// <summary>
    /// Event to pause and resume threads.
    /// </summary>
    private ManualResetEvent pauseThread = new ManualResetEvent(true); 

    /// <summary>
    /// Lock object for critical section.
    /// </summary>
    private readonly object criticalSectionLock = new object();

    /// <summary>
    /// Time for which threads sleep between ball creations.
    /// </summary>
    private int threadsSleepTime;

    /// <summary>
    /// Time decrement for each subsequent ball creation.
    /// </summary>
    private int timeDecreament;

    /// <summary>
    /// Total number of threads to be used for ball creation.
    /// </summary>
    private static int totalThreads = 50;

    /// <summary>
    /// Tracks the number of threads that have completed their execution.
    /// </summary>
    private int threadsCompleted; // Tracks thread completion

    /// <summary>
    /// Reference to the Game Completed Menu UI.
    /// </summary>
    public GameObject GameCompletedMenu; // Game completed Menu

    /// <summary>
    /// Reference to the Game Pause Button UI.
    /// </summary>
    public GameObject GamePauseButton;

    /// <summary>
    /// Indicates if the game is completed.
    /// </summary>
    public static bool GameCompleted = false;

    /// <summary>
    /// Score attained by the player.
    /// </summary>
    public static int AttainedScore;

    /// <summary>
    /// Instance of the SqlDatabase class for database interactions.
    /// </summary>
    private SqlDatabase sqlDatabaseInstance;

    /// <summary>
    /// Initializes the ball generator and sets up initial parameters.
    /// </summary>
    void Start()
    {
        sqlDatabaseInstance = new SqlDatabase();
        threadsSleepTime = BallConstants.GetThreadsSleepTimeForLevel();
        timeDecreament = BallConstants.GetThreadsSleepDecreamentForLevel();

        // Delay the start of ball creation
        Invoke("StartBallCreationWithDelay", 3.3f);
    }

    /// <summary>
    /// Starts the ball creation process with a delay.
    /// </summary>
    private void StartBallCreationWithDelay()
    {
        // Start the thread for ball creation
        ExecuteBallThreadsOnUnityMainThread.Instance();
        Thread ballThread = new Thread(() => CreateBallOnMainThread(6f, "Thread 1"));
        Thread ballThread2 = new Thread(() => CreateBallOnMainThread(6f, "Thread 2"));
        Thread ballThread3 = new Thread(() => CreateBallOnMainThread(6f, "Thread 3"));
        Thread ballThread4 = new Thread(() => CreateBallOnMainThread(6f, "Thread 4"));
        Thread ballThread5 = new Thread(() => CreateBallOnMainThread(6f, "Thread 5"));
        ballThread.Start();
        ballThread2.Start();
        ballThread3.Start();
        ballThread4.Start();
        ballThread5.Start();
    }

    /// <summary>
    /// Updates the game state and manages thread pausing/resuming.
    /// </summary>
    void Update()
    {
        if (GameCompleted || PauseGame.GamePaused)
        {
            Time.timeScale = 0;
            pauseThread.Reset();
        }
        else
        {
            Time.timeScale = 1;
            pauseThread.Set(); // Resume all threads
        }
    }

    /// <summary>
    /// Adds the function that creates the balls to the main Unity thread.
    /// </summary>
    /// <param name="y">The y-axis position of the ball.</param>
    /// <param name="threadId">The identifier of the thread.</param>
    private void CreateBallOnMainThread(float y, string threadId)
    {
        // Add the function to a list of functions to be executed in the main thread
        for (int i = 0; i < 10; i++)
        {
            try
            {
                semaphore.WaitOne();
                Debug.Log(threadId + " Acquire semaphore");
                pauseThread.WaitOne();  // Pause the thread if the game is paused

                lock (criticalSectionLock)
                {
                    Debug.Log(threadId + " Entering critical section");
                    ExecuteBallThreadsOnUnityMainThread.Instance().Enqueue(() => InstantiateBall(y, threadId));
                    Debug.Log(threadId + " Leaving critical section");
                }
            }
            finally
            {
                pauseThread.WaitOne(); // Pause the threads if the game is paused
                Thread.Sleep(threadsSleepTime);
                threadsSleepTime -= timeDecreament;
                semaphore.Release();
                Debug.Log(threadId + " Releases semaphore");
            }
        }
    }

    /// <summary>
    /// Instantiates a ball at the specified position.
    /// </summary>
    /// <param name="yPosition">The y-axis position of the ball.</param>
    /// <param name="threadId">The identifier of the thread.</param>
    private void InstantiateBall(float yPosition, string threadId)
    {
        float xPosition = Random.Range(-4.7f, 4.7f);
        GameObject newBall = Instantiate(ballPrefab, new Vector3(xPosition, yPosition, 0f), Quaternion.identity);
        BallMovement ballMovement = newBall.GetComponent<BallMovement>();
        // Increment threadsCompleted safely
        lock (this)
        {
            threadsCompleted++;
            if (threadsCompleted == totalThreads)
            {
                AllThreadsCompleted();
            }
        }
    }

    /// <summary>
    /// Called when all threads have completed their execution.
    /// </summary>
    private void AllThreadsCompleted()
    {
        // Once all threads complete, invoke scene transition
        Debug.Log("All threads completed. Loading next scene in 5 seconds.");
        StartCoroutine(LoadCompletedMenuWithDelay(5f));
    }

    /// <summary>
    /// Loads the game completed menu with a delay.
    /// </summary>
    /// <param name="delay">The delay before loading the menu.</param>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator LoadCompletedMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for 3 seconds
        GameCompleted = true;
        SqlDatabaseConstants.CompletedGameScore = AttainedScore;
        sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.RecordCompletedGameOperation);
        GameCompletedMenu.SetActive(true);
        GamePauseButton.SetActive(false);
        Player.SetActive(false);
    }
}
