using UnityEngine;

/// <summary>
/// Contains constant values related to player behavior and movement limits.
/// </summary>
public class PlayerConstants
{
    /// <summary>
    /// Speed of moving the player in either direction.
    /// </summary>
    public static readonly float MoveSpeed = 10f; // speed of moving the player either direction

    /// <summary>
    /// Maximum left position to move the player.
    /// </summary>
    public static readonly float LeftLimit = -4.75f;  // max left position to move the player

    /// <summary>
    /// Maximum right position to move the player.
    /// </summary>
    public static readonly float RightLimit = 4.75f; // max right position to move the player

    /// <summary>
    /// Maximum upper position to move the player.
    /// </summary>
    public static readonly float UpperLimit = 3.0f; // max upper position to move the player

    /// <summary>
    /// Maximum bottom position to move the player.
    /// </summary>
    public static readonly float BottomLimit = -4.75f; // max bottom position to move the player

    /// <summary>
    /// Delay before destroying the falling ball after hitting.
    /// </summary>
    public static readonly float DestructionDelay = 1f;  // speed of destroying the falling ball after hitting
}
