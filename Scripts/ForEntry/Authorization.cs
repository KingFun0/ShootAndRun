using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class Authorization : MonoBehaviour
{
    public SqliteConnection dbConnection;
    private string path;
    public static string newLogin;
    public TMP_InputField loginInputField;
    public TMP_InputField passwordInputField;
    public Button registerButton;
    public TMP_Text errorMessageText;
    public GameObject panel;
    private void Awake()
    {
        SetConnection();
        CheckIfDatabaseIsEmpty();
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.inputType = TMP_InputField.InputType.Password;

    }


    private void CheckIfDatabaseIsEmpty()
    {
        path = Application.dataPath + "/BD/DataBase/db.db";
        dbConnection = new SqliteConnection("URI=file:" + path);

        using (SqliteCommand command = new SqliteCommand("SELECT COUNT(*) FROM lap WHERE login IS NOT NULL AND password IS NOT NULL", dbConnection))
        {
            dbConnection.Open();
            int count = Convert.ToInt32(command.ExecuteScalar());
            Debug.Log(count);
            if (count == 0)
            {
                errorMessageText.text = "База данных пуста.";
            }
        }
    }


    public void SetConnection()
    {
        path = Application.dataPath + "/BD/DataBase/db.db";
        dbConnection = new SqliteConnection("URI=file:" + path);

        dbConnection.Open();
        if (dbConnection.State == ConnectionState.Open)
        {
            Debug.Log("Успешное подключение к базе данных");
        }
        else
        {
            Debug.Log("Ошибка подключения");
        }
    }
    private bool AuthenticateUser(string login, string password)
    {
        path = Application.dataPath + "/BD/DataBase/db.db";
        dbConnection = new SqliteConnection("URI=file:" + path);
        string selectQuery = "SELECT COUNT(*) FROM lap WHERE login = @login AND password = @password";

        using (SqliteCommand cmd = new SqliteCommand(selectQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", password);

            dbConnection.Open();
            object result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                int count;
                if (int.TryParse(result.ToString(), out count))
                {
                    return count > 0;
                }
                else
                {
                    Debug.LogError("Неверный результат запроса к базе данных: " + result.ToString());
                }
            }
            else
            {
                Debug.LogError("Нет результата от запроса к базе данных");
            }

            return false;

        }

    }



    private void OnRegisterButtonClick()
    {
        string login = loginInputField.text;
        string password = passwordInputField.text;
       
        bool isAuthenticated = AuthenticateUser(login, password);


        if (isAuthenticated)
        {
            
            newLogin = login;
            MyLogin.login = newLogin;


            Debug.Log("Успешно!");
            SceneManager.LoadScene(4);
            Load.SetSceneID(3);
            return;
        }
        else
        {
            errorMessageText.text = "Проверьте корректность веденных данных!";
            panel.SetActive(true);
        }

    }

}