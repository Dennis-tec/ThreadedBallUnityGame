using System;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// Executes functions on the Unity main thread that are enqueued by background threads.
/// </summary>
public class ExecuteBallThreadsOnUnityMainThread : MonoBehaviour
{
    /// <summary>
    /// Queue to hold functions to be executed. The queue contains functions that have been added by threads running in the background.
    /// </summary>
    private static readonly ConcurrentQueue<Action> FunctionsToExecute = new ConcurrentQueue<Action>();

    /// <summary>
    /// Singleton instance of ExecuteBallThreadsOnUnityMainThread.
    /// </summary>
    private static ExecuteBallThreadsOnUnityMainThread _instance;

    /// <summary>
    /// Ensures that only one instance of ExecuteBallThreadsOnUnityMainThread is running.
    /// </summary>
    /// <returns>The singleton instance of ExecuteBallThreadsOnUnityMainThread.</returns>
    public static ExecuteBallThreadsOnUnityMainThread Instance()
    {
        if (_instance == null)
        {
            // Create a new instance of ExecuteBallThreadsOnUnityMainThread using its game object
            var newInstance = new GameObject("ExecuteBallThreadsOnUnityMainThread");
            _instance = newInstance.AddComponent<ExecuteBallThreadsOnUnityMainThread>();  // attach component to this new instance
        }
        return _instance;
    }

    /// <summary>
    /// Enqueues a function to be executed on the main thread.
    /// </summary>
    /// <param name="action">The function to be executed.</param>
    public void Enqueue(Action action)
    {
        FunctionsToExecute.Enqueue(action);
    }

    /// <summary>
    /// Called once per frame. Dequeues actions (which are functions) and executes them on the main thread.
    /// </summary>
    void Update()
    {
        while (FunctionsToExecute.TryDequeue(out var function))
        {
            function.Invoke();
        }
    }
}
