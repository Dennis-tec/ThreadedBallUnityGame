using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages user input and interactions for registering and selecting users.
/// </summary>
public class UserInputs : MonoBehaviour
{
    /// <summary>
    /// Input field for entering a new username.
    /// </summary>
    public TMP_InputField inputedUserName;

    /// <summary>
    /// Text component for displaying input messages.
    /// </summary>
    public TextMeshProUGUI inputMessage;

    /// <summary>
    /// Dropdown for selecting existing users.
    /// </summary>
    public TMP_Dropdown usersDropDown;

    /// <summary>
    /// Stores the new user input.
    /// </summary>
    public static string NewUserInput;

    /// <summary>
    /// UI element for the home button menu.
    /// </summary>
    public GameObject homeButtonMenu;

    /// <summary>
    /// UI element for the user input menu.
    /// </summary>
    public GameObject userInputMenu;

    /// <summary>
    /// UI element for the quit button.
    /// </summary>
    public GameObject quitButton;

    /// <summary>
    /// UI element for the existing users menu.
    /// </summary>
    public GameObject existingUsersMenu;

    /// <summary>
    /// UI element for displaying registration messages.
    /// </summary>
    public GameObject registerationMessageObject;

    /// <summary>
    /// Image component for the user icon input.
    /// </summary>
    public Image userIconInput;

    /// <summary>
    /// Instance of the SqlDatabase class for database interactions.
    /// </summary>
    private SqlDatabase sqlDatabaseInstance = new SqlDatabase();

    /// <summary>
    /// Index of the selected username in the dropdown.
    /// </summary>
    private static int selectedUserNameIndex;

    /// <summary>
    /// Registers a new username and logs in the user.
    /// </summary>
    public void RegisterNewUserNameAndLogin()
    {
        if (inputedUserName.text.Length < 5)
        {
            // Display message that the username is too short
            inputMessage.text = "UserName must have more than 4 characters";
            registerationMessageObject.SetActive(true);
        }
        else
        {
            NewUserInput = inputedUserName.text;
            sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.RegisterNewPlayerOperation);

            if (SqlDatabaseConstants.UserNameNotRegisteredAlready)
            {
                inputMessage.text = "UserName Created Successfully!";
                inputMessage.color = Color.green;
                registerationMessageObject.SetActive(true);
                StateTransitions.GoToMainMenu();
            }
            else
            {
                inputMessage.text = "UserName Already Registered!";
                registerationMessageObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Populates the dropdown with existing usernames from the database.
    /// </summary>
    public void PopulateDropDownWithExistingUserNames()
    {
        Sprite userIconSprite = userIconInput.sprite;
        var userOptions = new List<TMP_Dropdown.OptionData>();
        sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.GetRegisteredPlayersOperation);
        foreach (var item in SqlDatabaseConstants.ListOfRegisteredUsers)
        {
            userOptions.Add(item: new TMP_Dropdown.OptionData(text: item, image: userIconSprite, color: Color.white));
        }
        usersDropDown.AddOptions(userOptions);
        usersDropDown.RefreshShownValue();
    }

    /// <summary>
    /// Modifies the selected dropdown value index.
    /// </summary>
    public void ModifySelectedDropDownValueIndex()
    {
        selectedUserNameIndex = usersDropDown.value;
        //selectedUserName = usersDropDown.options[pickIndex].text;
    }

    /// <summary>
    /// Navigates to the create new username menu.
    /// </summary>
    public void GoToCreateNewUserName()
    {
        homeButtonMenu.SetActive(false);
        quitButton.SetActive(false);
        userInputMenu.SetActive(true);
    }

    /// <summary>
    /// Returns to the home menu from the user interaction menu.
    /// </summary>
    public void ReturnToHomeMenuFromUserInteractionMenu()
    {
        homeButtonMenu.SetActive(true);
        quitButton.SetActive(true);
        userInputMenu.SetActive(false);
        existingUsersMenu.SetActive(false);
    }

    /// <summary>
    /// Navigates to the existing usernames menu.
    /// </summary>
    public void SelectFromExistingUserNames()
    {
        PopulateDropDownWithExistingUserNames();
        homeButtonMenu.SetActive(false);
        quitButton.SetActive(false);
        existingUsersMenu.SetActive(true);
    }

    /// <summary>
    /// Uses the selected username from the dropdown.
    /// </summary>
    public void UseSelectedUserName()
    {
        ModifySelectedDropDownValueIndex();
        SqlDatabaseConstants.ActiveUserId = 0;
        SqlDatabaseConstants.ActiveUserName = usersDropDown.options[selectedUserNameIndex].text;
        sqlDatabaseInstance.InteractWithDatabase(SqlDatabaseConstants.GetUserNameIdOperations);
        StateTransitions.GoToMainMenu();
    }

    /// <summary>
    /// Logs in as a guest user.
    /// </summary>
    public void PlayAsGuest()
    {
        SqlDatabaseConstants.ActiveUserId = 0;
        SqlDatabaseConstants.ActiveUserName = SqlDatabaseConstants.GuestUserName;
        StateTransitions.GoToMainMenu();
    }
}
