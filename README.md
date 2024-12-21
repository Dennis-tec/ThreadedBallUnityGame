Game is only played on windows. Follow these steps to play the game:
* Git clone this repo.
* Install UNITY 6(6000.0.24f1).
* Install the latest version of Visual Studio.
* Install the latest version of Microsoft SQL Server Management Studio
* Create a Microsoft SQL Server Database.
* Enable SQL Server authentication for your new database.
* Modify the connection string in line 37 of Assets/Scripts/SqlDatabase.cs file by replace the connection string with your database details: string connectionString = $"Server={ServerName};Database={DatabaseName};User ID={User ID};Password={Password};TrustServerCertificate=True;"
* Follow the instructions from the Manual to learn how to play once your game is running.
