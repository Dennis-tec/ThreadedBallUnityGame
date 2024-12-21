using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Contains constant values related to SQL database operations.
/// </summary>
public class SqlDatabaseConstants
{
    // Server, Database, and Table Constraints

    /// <summary>
    /// Name of the database.
    /// </summary>
    public static readonly string DatabaseName = "DennisDB";

    /// <summary>
    /// Name of the server.
    /// </summary>
    public static readonly string ServerName = "LAPTOP-EL2VMTQN\\SQLEXPRESS";

    /// <summary>
    /// Name of the players table.
    /// </summary>
    public static readonly string PlayersTableName = "ThreadBallsPlayers";

    /// <summary>
    /// Name of the games table.
    /// </summary>
    public static readonly string GamesTableName = "ThreadedBallsGameRecords";

    /// <summary>
    /// Default guest username.
    /// </summary>
    public static readonly string GuestUserName = "Guest";

    // Database Interaction Operations constants

    /// <summary>
    /// Operation code for creating tables.
    /// </summary>
    public static readonly int CreateTablesOperation = 0;

    /// <summary>
    /// Operation code for registering a new player.
    /// </summary>
    public static readonly int RegisterNewPlayerOperation = 1;

    /// <summary>
    /// Operation code for recording a completed game.
    /// </summary>
    public static readonly int RecordCompletedGameOperation = 2;

    /// <summary>
    /// Operation code for getting registered players.
    /// </summary>
    public static readonly int GetRegisteredPlayersOperation = 3;

    /// <summary>
    /// Operation code for getting the user ID.
    /// </summary>
    public static readonly int GetUserNameIdOperations = 4;

    /// <summary>
    /// Operation code for getting the highest score.
    /// </summary>
    public static readonly int GetHighestScoreOperation = 5;

    // Database Variables to update

    /// <summary>
    /// List of registered users from the database.
    /// </summary>
    public static List<string> ListOfRegisteredUsers;

    /// <summary>
    /// Recorded high score from the database.
    /// </summary>
    public static int RecordedHighScore;

    /// <summary>
    /// Score attained upon game completion.
    /// </summary>
    public static int CompletedGameScore;

    /// <summary>
    /// Active username in the game.
    /// </summary>
    public static string ActiveUserName;

    /// <summary>
    /// Active user ID in the game.
    /// </summary>
    public static int ActiveUserId;

    // New User Registration

    /// <summary>
    /// Indicates if the username is not already registered.
    /// </summary>
    public static bool UserNameNotRegisteredAlready;
}
