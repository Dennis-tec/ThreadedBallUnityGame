using UnityEngine;

/// <summary>
/// Contains constant values and methods related to ball behavior and game levels.
/// </summary>
public class BallConstants
{
    /// <summary>
    /// Minimum y-coordinate before the ball disappears out of the game scene.
    /// </summary>
    public static readonly float minimumY = -5.1f;  // Minimum y before the ball disappears out of the game scene

    /// <summary>
    /// Gravity scale for Level 1 (Easy).
    /// </summary>
    public static readonly float EasyLevelGravity = 1.0f;  // Gravity scale for Level 1 (Easy)

    /// <summary>
    /// Gravity scale for Level 2 (Medium).
    /// </summary>
    public static readonly float MediumLevelGravity = 1.25f;  // Gravity scale for Level 2 (Medium)

    /// <summary>
    /// Gravity scale for Level 3 (Hard).
    /// </summary>
    public static readonly float HardLevelGravity = 1.5f;  // Gravity scale for Level 3 (Hard)

    /// <summary>
    /// Initial sleep time for a thread for Level 1 (Easy).
    /// </summary>
    public static readonly int EasyLevelThreadsSleepTime = 3000;

    /// <summary>
    /// Initial sleep time for a thread for Level 2 (Medium).
    /// </summary>
    public static readonly int MediumLevelThreadsSleepTime = 2800;

    /// <summary>
    /// Initial sleep time for a thread for Level 3 (Hard).
    /// </summary>
    public static readonly int HardLevelThreadsSleepTime = 2600;

    /// <summary>
    /// Value to decrement each sleep time per thread for Level 1 (Easy).
    /// </summary>
    public static readonly int EasyLevelDecreamentThreadsSleepTime = 10;

    /// <summary>
    /// Value to decrement each sleep time per thread for Level 2 (Medium).
    /// </summary>
    public static readonly int MediumLevelDecreamentThreadsSleepTime = 20;

    /// <summary>
    /// Value to decrement each sleep time per thread for Level 3 (Hard).
    /// </summary>
    public static readonly int HardLevelDecreamentThreadsSleepTime = 30;

    /// <summary>
    /// Name to be displayed for Level 1 (Easy).
    /// </summary>
    public static readonly string EasyLevelName = "Level 1(Easy)";

    /// <summary>
    /// Name to be displayed for Level 2 (Medium).
    /// </summary>
    public static readonly string MediumLevelName = "Level 2(Medium)";

    /// <summary>
    /// Name to be displayed for Level 3 (Hard).
    /// </summary>
    public static readonly string HardLevelName = "Level 3(Hard)";

    /// <summary>
    /// Current game level.
    /// </summary>
    public static int _gameLevel = 1;

    /// <summary>
    /// Gets the gravity scale based on the current game level.
    /// </summary>
    /// <returns>Gravity scale for the current level.</returns>
    public static float GetGravityForLevel()
    {
        switch (_gameLevel)
        {
            case 1:
                return EasyLevelGravity;
            case 2:
                return MediumLevelGravity;
            case 3:
                return HardLevelGravity;
            default:
                return EasyLevelGravity; // Default to Easy level gravity if no selection
        }
    }

    /// <summary>
    /// Gets the thread sleep time based on the current game level.
    /// </summary>
    /// <returns>Thread sleep time for the current level.</returns>
    public static int GetThreadsSleepTimeForLevel()
    {
        switch (_gameLevel)
        {
            case 1:
                return EasyLevelThreadsSleepTime;
            case 2:
                return MediumLevelThreadsSleepTime;
            case 3:
                return HardLevelThreadsSleepTime;
            default:
                return EasyLevelThreadsSleepTime; // Default to Easy level Threads Sleep Time if no selection
        }
    }

    /// <summary>
    /// Gets the thread sleep decrement value based on the current game level.
    /// </summary>
    /// <returns>Thread sleep decrement value for the current level.</returns>
    public static int GetThreadsSleepDecreamentForLevel()
    {
        switch (_gameLevel)
        {
            case 1:
                return EasyLevelDecreamentThreadsSleepTime;
            case 2:
                return MediumLevelDecreamentThreadsSleepTime;
            case 3:
                return HardLevelDecreamentThreadsSleepTime;
            default:
                return EasyLevelDecreamentThreadsSleepTime; // Default to Easy level threads decreament if no selection
        }
    }

    /// <summary>
    /// Gets the name of the current game level.
    /// </summary>
    /// <returns>Name of the current level.</returns>
    public static string GetNameForLevel()
    {
        switch (_gameLevel)
        {
            case 1:
                return EasyLevelName;
            case 2:
                return MediumLevelName;
            case 3:
                return HardLevelName;
            default:
                return EasyLevelName; // Default to Easy level threads decreament if no selection
        }
    }
}
