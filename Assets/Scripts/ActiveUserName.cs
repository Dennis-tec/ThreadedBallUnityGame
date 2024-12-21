using TMPro;
using UnityEngine;

/// <summary>
/// Displays the active username in the game.
/// </summary>
public class ActiveUserName : MonoBehaviour
{
    /// <summary>
    /// UI text component for displaying the username.
    /// </summary>
    public TextMeshProUGUI userName;

    /// <summary>
    /// Initializes the active username display.
    /// </summary>
    void Start()
    {
        userName.text = SqlDatabaseConstants.ActiveUserName;
    }
}
