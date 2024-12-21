using System;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// Executes functions on the Unity main thread that are enqueued by background threads.
/// </summary>
public class ExecuteCoinThreadsOnUnityMainThread : MonoBehaviour
{
    /// <summary>
    /// Queue to hold functions to be executed. The queue contains functions that have been added by threads running in the background.
    /// </summary>
    private static readonly ConcurrentQueue<Action> FunctionsToExecute = new ConcurrentQueue<Action>();

    /// <summary>
    /// Singleton instance of ExecuteCoinThreadsOnUnityMainThread.
    /// </summary>
    private static ExecuteCoinThreadsOnUnityMainThread _instance;

    /// <summary>
    /// Ensures that only one instance of ExecuteCoinThreadsOnUnityMainThread is running.
    /// </summary>
    /// <returns>The singleton instance of ExecuteCoinThreadsOnUnityMainThread.</returns>
    public static ExecuteCoinThreadsOnUnityMainThread Instance()
    {
        if (_instance == null)
        {
            // Create a new instance of ExecuteCoinThreadsOnUnityMainThread using its game object
            var newInstance = new GameObject("ExecuteCoinThreadsOnUnityMainThread");
            _instance = newInstance.AddComponent<ExecuteCoinThreadsOnUnityMainThread>();  // attach component to this new instance
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
