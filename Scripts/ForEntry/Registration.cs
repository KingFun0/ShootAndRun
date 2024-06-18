using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class Registration : MonoBehaviour
{
    public SqliteConnection dbConnection;
    private string path;

    public TMP_InputField loginInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordInputField2;
    public Button registerButton;
    public TMP_Text errorMessageText;
    public GameObject panel;
    private void Awake()
    {
        SetConnection();
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.inputType = TMP_InputField.InputType.Password;
        passwordInputField2.contentType = TMP_InputField.ContentType.Password;
        passwordInputField2.inputType = TMP_InputField.InputType.Password;

    }

    public void SetConnection()
    {
        path = Application.dataPath + "/BD/DataBase/db.db"; 

        dbConnection = new SqliteConnection("URI=file:" + path);
        dbConnection.Open();
        if (dbConnection.State == ConnectionState.Open)
        {
            Debug.Log("�������� ����������� � ���� ������");
        }
        else
        {
            Debug.Log("������ �����������");
        }
    }
    private void TrueOrFalse()
    {
        string insertQuery = "INSERT INTO lal (login, roundswin) VALUES (@login, @roundswin)";
        string login = loginInputField.text;
        using (SqliteCommand cmd = new SqliteCommand(insertQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@roundswin", 0);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Debug.Log("������������ ������� ���������������.");
            }
            else
            {
                Debug.LogError("����������� ������������ �� �������.");
            }
        }
    }
    private void Score()
    {
        string insertQuery = "INSERT INTO las (login, score) VALUES (@login, @score)";
        string login = loginInputField.text;
        using (SqliteCommand cmd = new SqliteCommand(insertQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@score", 0);
    

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Debug.Log("������������ ������� ���������������.");
            }
            else
            {
                Debug.LogError("����������� ������������ �� �������.");
            }
        }
    }
    private void RegisterUser()
    {
        //SQL �������
        string insertQuery = "INSERT INTO lap (login, password) VALUES (@login, @password)";
        string selectQuery = "SELECT COUNT(*) FROM lap WHERE login = @login";
        //SQL �������

        string login = loginInputField.text;
        string password = passwordInputField.text;
        string password2 = passwordInputField2.text;
        if (dbConnection == null || dbConnection.State != ConnectionState.Open)
        {
            Debug.LogError("���������� � ����� ������ �� �������.");
            return;
        }



        if (password.Length < 5)
        {
            errorMessageText.text = "����� ������ ������ 5 ��������.";
            panel.SetActive(true);
            return;
        }

        if (password != password2)
        {
            errorMessageText.text = "������ �� ���������.";
            panel.SetActive(true);
            return;
        }

        using (SqliteCommand cmd = new SqliteCommand(selectQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                errorMessageText.text = "������������ � ����� ������� ��� ����������.";
                panel.SetActive(true);
                return;
            }
        }

       

        using (SqliteCommand cmd = new SqliteCommand(insertQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", password);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Debug.Log("������������ ������� ���������������.");
                TrueOrFalse();
                Score();
                SceneManager.LoadScene(4);
                Load.SetSceneID(3);
            }
            else
            {
                Debug.LogError("����������� ������������ �� �������.");
            }
        }
        MyLogin.login = login;
    }


    private void OnRegisterButtonClick()
    {
            
            RegisterUser();
        
    }
}
