using UnityEngine;

/// <summary>
/// Creates the necessary SQL database tables when the game starts.
/// </summary>
public class CreateSqlDatabaseTables : MonoBehaviour
{
    /// <summary>
    /// Instance of the SqlDatabase class for database interactions.
    /// </summary>
    private SqlDatabase sqlDatabaseInstance;

    /// <summary>
    /// Initializes the database tables at the start of the game.
    /// </summary>
    void Start()
    {
        sqlDatabaseInstance = new SqlDatabase();
        sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.CreateTablesOperation);
    }
}
