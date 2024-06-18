using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using Mono.Data.Sqlite;
using System.Data;
public class PlayerCountRounds : MonoBehaviour
{
    [SerializeField] private TMP_Text _countWinRoundsForPlayer;
    [SerializeField] private TMP_Text _ratingPlayer;
    public SqliteConnection dbConnection;
    private string path;
    private void Awake()
    {
        SetConnection();
        UpdateRoundsWinUI(MyLogin.login);
        UpdateRating(MyLogin.login);
    }
    public void SetConnection()
    {
        path = Application.dataPath + "/BD/DataBase/db.db";

        dbConnection = new SqliteConnection("URI=file:" + path);
        dbConnection.Open();
        if (dbConnection.State == ConnectionState.Open)
        {
            Debug.Log("Successful connection to the database");
        }
        else
        {
            Debug.Log("Connection error");
        }
    }
    private void UpdateRoundsWinUI(string login)
    {
        string selectQuery = "SELECT roundswin FROM lal WHERE login = @login";

        using (SqliteCommand cmd = new SqliteCommand(selectQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            using (SqliteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int roundsWin = reader.GetInt32(0);
                    _countWinRoundsForPlayer.text = roundsWin.ToString();
                }
            }
        }
    }
    private void UpdateRating(string login)
    {
        string selectQuery = "SELECT score FROM las WHERE login = @login";

        using (SqliteCommand cmd = new SqliteCommand(selectQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            using (SqliteDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int roundsWin = reader.GetInt32(0);
                    _ratingPlayer.text = roundsWin.ToString();
                }
            }
        }
    }
}
