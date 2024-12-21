using UnityEngine;

/// <summary>
/// Manages the game's pause and resume functionality.
/// </summary>
public class PauseGame : MonoBehaviour
{
    /// <summary>
    /// UI element for the game paused menu.
    /// </summary>
    public GameObject GamePausedMenu;

    /// <summary>
    /// UI element for the game pause button.
    /// </summary>
    public GameObject GamePauseButton;

    /// <summary>
    /// Reference to the player GameObject.
    /// </summary>
    public GameObject Player;

    /// <summary>
    /// Indicates if the game is currently paused.
    /// </summary>
    public static bool GamePaused = false;

    /// <summary>
    /// Pauses the game and shows the paused menu.
    /// </summary>
    public void StopGame()
    {
        GamePaused = true;
        GamePausedMenu.SetActive(true);
        GamePauseButton.SetActive(false);
        Player.SetActive(false);
    }

    /// <summary>
    /// Resumes the game and hides the paused menu.
    /// </summary>
    public void ResumeGame()
    {
        GamePaused = false;
        GamePausedMenu.SetActive(false);
        GamePauseButton.SetActive(true);
        Player.SetActive(true);
    }
}
