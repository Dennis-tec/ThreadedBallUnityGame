using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interactions with the SQL database.
/// </summary>
public class SqlDatabase
{
    /// <summary>
    /// Name of the database.
    /// </summary>
    private string DatabaseName = SqlDatabaseConstants.DatabaseName;

    /// <summary>
    /// Name of the server.
    /// </summary>
    private string ServerName = SqlDatabaseConstants.ServerName;

    /// <summary>
    /// Name of the players table.
    /// </summary>
    private string playersTable = SqlDatabaseConstants.PlayersTableName;

    /// <summary>
    /// Name of the games record table.
    /// </summary>
    private string gamesRecordTable = SqlDatabaseConstants.GamesTableName;

    /// <summary>
    /// Interacts with the database based on the specified operation.
    /// </summary>
    /// <param name="operation">Operation code.</param>
    public void InteractWithDatabase(int operation)
    {
        string connectionString = $"Server={ServerName};Database={DatabaseName};User ID=Dyiaile;Password=Yiaile6345!;TrustServerCertificate=True;";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Debug.Log("Successfully Connected to the Database!");
                if (operation == SqlDatabaseConstants.CreateTablesOperation)
                {
                    CreateTables(connection, playersTable, gamesRecordTable);
                }
                else if (operation == SqlDatabaseConstants.RegisterNewPlayerOperation)
                {
                    RegisterNewPlayerInTheDatabase(connection, playersTable, UserInputs.NewUserInput);
                }
                else if (operation == SqlDatabaseConstants.RecordCompletedGameOperation)
                {
                    RecordCompletedGameInTheDatabase(connection, gamesRecordTable, BallConstants.GetNameForLevel(), SqlDatabaseConstants.CompletedGameScore);
                }
                else if (operation == SqlDatabaseConstants.GetRegisteredPlayersOperation)
                {
                    GetRegisteredPlayers(connection, playersTable);
                }
                else if (operation == SqlDatabaseConstants.GetHighestScoreOperation)
                {
                    GetHighScoresFromTheDatabase(connection, gamesRecordTable, BallConstants.GetNameForLevel());
                }
                else if (operation == SqlDatabaseConstants.GetUserNameIdOperations)
                {
                    GetUserNameId(connection, playersTable, SqlDatabaseConstants.ActiveUserName);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// Creates the necessary tables in the database.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="playersTableName">Name of the players table.</param>
    /// <param name="gamesTableName">Name of the games table.</param>
    public void CreateTables(SqlConnection connection, string playersTableName, string gamesTableName)
    {
        string createTableQuery = $@"
                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{playersTableName}')
                            BEGIN
                            CREATE TABLE {playersTableName} (
                                Player_Id INT IDENTITY(1,1) PRIMARY KEY,
                                Player_Name NVARCHAR(50));
                            END;
                            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{gamesTableName}')
                            BEGIN
                            CREATE TABLE {gamesTableName} (
                                Game_Id INT IDENTITY(1,1) PRIMARY KEY,
                                Level_Name NVARCHAR(50),
                                Player_Id INT,
                                CONSTRAINT FK_Player_Id FOREIGN KEY (Player_Id)
                                    REFERENCES {playersTableName}(Player_Id),
                                Scores INT);
                            END";

        ExecuteQuery(connection, createTableQuery);
    }

    /// <summary>
    /// Retrieves high scores from the database.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="gamesTable">Name of the games table.</param>
    /// <param name="levelName">Name of the level.</param>
    public void GetHighScoresFromTheDatabase(SqlConnection connection, string gamesTable, string levelName)
    {
        string maxScoreQuery = $@"SELECT ISNULL(MAX(Scores), 0) AS MaxScore FROM {gamesTable} where Level_Name = '{levelName}'";

        using (SqlCommand command = new SqlCommand(maxScoreQuery, connection))
        {
            object result = command.ExecuteScalar();
            SqlDatabaseConstants.RecordedHighScore = Convert.ToInt32(result);
        }
    }

    /// <summary>
    /// Registers a new player in the database.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="playersTable">Name of the players table.</param>
    /// <param name="PlayersName">Name of the player.</param>
    public void RegisterNewPlayerInTheDatabase(SqlConnection connection, string playersTable, string PlayersName)
    {
        string checkIfUserExist = $@"SELECT COUNT(1) FROM {playersTable} WHERE Player_Name = '{PlayersName}'";
        string insterUserQuery = $@"INSERT INTO {playersTable} (Player_Name) VALUES ('{PlayersName}')";

        bool userNameAlreadyExist = QueryCheckIfPlayerExist(connection, checkIfUserExist);
        if (userNameAlreadyExist)
        {
            SqlDatabaseConstants.UserNameNotRegisteredAlready = false;
        }
        else
        {
            SqlDatabaseConstants.UserNameNotRegisteredAlready = true;

            ExecuteQuery(connection, insterUserQuery);
            SqlDatabaseConstants.ActiveUserName = PlayersName;
            InteractWithDatabase(SqlDatabaseConstants.GetUserNameIdOperations);
        }
    }

    /// <summary>
    /// Retrieves the user ID for the specified player.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="playersTable">Name of the players table.</param>
    /// <param name="playersName">Name of the player.</param>
    public void GetUserNameId(SqlConnection connection, string playersTable, string playersName)
    {
        string getNewUserId = $@"SELECT Player_Id FROM {playersTable} where Player_Name = '{playersName}'";
        using (SqlCommand command = new SqlCommand(getNewUserId, connection))
        {
            object result = command.ExecuteScalar();
            SqlDatabaseConstants.ActiveUserId = Convert.ToInt32(result);
        }
    }

    /// <summary>
    /// Records a completed game in the database.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="gamesTable">Name of the games table.</param>
    /// <param name="levelName">Name of the level.</param>
    /// <param name="gameScore">Score of the game.</param>
    public void RecordCompletedGameInTheDatabase(SqlConnection connection, string gamesTable, string levelName, int gameScore)
    {
        string insertGameRecordQuery = $"INSERT INTO {gamesTable} (Level_Name, Scores) VALUES ('{levelName}', '{gameScore}')";
        if (!string.Equals(SqlDatabaseConstants.ActiveUserName, SqlDatabaseConstants.GuestUserName, StringComparison.OrdinalIgnoreCase))
        {
            insertGameRecordQuery = $"INSERT INTO {gamesTable} (Level_Name, Player_Id, Scores) VALUES ('{levelName}', '{SqlDatabaseConstants.ActiveUserId}', '{gameScore}')";
        }
        ExecuteQuery(connection, insertGameRecordQuery);
    }

    /// <summary>
    /// Retrieves the list of registered players from the database.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="playersTable">Name of the players table.</param>
    public void GetRegisteredPlayers(SqlConnection connection, string playersTable)
    {
        List<String> playerNames = new List<String>();
        string listOfPlayersQuery = $@"SELECT Player_Name FROM {playersTable}";

        using (SqlCommand command = new SqlCommand(listOfPlayersQuery, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Add the value of the Player_Name column to the list
                    playerNames.Add(reader["Player_Name"].ToString());
                }
            }
        }
        SqlDatabaseConstants.ListOfRegisteredUsers = playerNames;
    }

    /// <summary>
    /// Executes the specified SQL query.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="query">SQL query.</param>
    private void ExecuteQuery(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Checks if a player exists in the database based on the specified query.
    /// </summary>
    /// <param name="connection">SQL connection.</param>
    /// <param name="query">SQL query.</param>
    /// <returns>True if the player exists, otherwise false.</returns>
    private bool QueryCheckIfPlayerExist(SqlConnection connection, string query)
    {
        using (SqlCommand command = new SqlCommand(query, connection))
        {
            int count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }
}
