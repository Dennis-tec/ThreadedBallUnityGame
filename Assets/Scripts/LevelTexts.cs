using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display of level name and countdown text.
/// </summary>
public class LevelTexts : MonoBehaviour
{
    /// <summary>
    /// UI text component for displaying the level name.
    /// </summary>
    public TextMeshProUGUI levelName;

    /// <summary>
    /// UI text component for displaying the countdown.
    /// </summary>
    public TextMeshProUGUI counterText;  // Counter text for countdown

    /// <summary>
    /// Initializes the level texts and starts the countdown.
    /// </summary>
    public void Start()
    {
        levelName.text = BallConstants.GetNameForLevel();  // Get Level Name
        StartCoroutine(StartCountdown());
    }

    /// <summary>
    /// Displays a countdown from 3 to 1 and then destroys the texts.
    /// </summary>
    /// <returns>An IEnumerator for the coroutine.</returns>
    private IEnumerator StartCountdown()
    {
        // Display countdown from 3 to 1
        for (int i = 3; i > 0; i--)
        {
            counterText.text = i.ToString();  // Update counter text
            yield return new WaitForSeconds(1f);  // Wait for 1 second
        }

        // Destroy texts after 3 seconds for the game to start
        Destroy(levelName.gameObject);
        Destroy(counterText.gameObject);
    }
}
