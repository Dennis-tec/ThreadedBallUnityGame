using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages state transitions in the game.
/// </summary>
public class StateTransitions : MonoBehaviour
{
    /// <summary>
    /// Loads the game play scene.
    /// </summary>
    public void PlayButton()
    {
        SceneManager.LoadScene("Game Play");
    }

    /// <summary>
    /// Loads the settings scene.
    /// </summary>
    public void MoveToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// Loads the home scene.
    /// </summary>
    public void ReturnHome()
    {
        SceneManager.LoadScene("Home");
    }

    /// <summary>
    /// Quits the game application.
    /// </summary>
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    /// <summary>
    /// Loads the main menu scene and resets game states.
    /// </summary>
    public static void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        PauseGame.GamePaused = false;
        BallsGenerator.GameCompleted = false;
    }

    /// <summary>
    /// Sets the game level.
    /// </summary>
    /// <param name="level">The level to set.</param>
    public void LevelSelection(int level)
    {
        BallConstants._gameLevel = level;
    }

    /// <summary>
    /// Restarts the game and resets game states.
    /// </summary>
    public void RestartGame()
    {
        PlayButton();
        PauseGame.GamePaused = false;
        BallsGenerator.GameCompleted = false;
    }

    /// <summary>
    /// Advances to the next level or loops back to the first level if the last level is reached.
    /// </summary>
    public void NextLevel()
    {
        if (BallConstants._gameLevel >= 3)
        {
            LevelSelection(1);
        }
        else
        {
            BallConstants._gameLevel += 1;
        }
        PlayButton();
        BallsGenerator.GameCompleted = false;
    }
}
