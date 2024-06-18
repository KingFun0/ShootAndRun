using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.IO;
using System.Data.Common;
using System.Data;
using UnityEngine.SocialPlatforms.Impl;
using System;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System.EnterpriseServices;

public class GameControll : MonoBehaviourPunCallbacks
{
    public SqliteConnection dbConnection;
    private string path;

    [SerializeField] private TMP_Text _time;
    [SerializeField] private TMP_Text _scoreCT;
    [SerializeField] private TMP_Text _scoreT;
    [SerializeField] private Canvas _endGame;

    private int _timeSecond = 45;
    private int _scoreCTValue = 0;
    private int _scoreTValue = 0;
    private bool isRoundRestarting = false;
    private int _randomRating = 2;

    private PhotonView _photonView;
    static public float x, y, z;

    private bool isGameend = false;
    private float elapsedTime = 0f;
    private float waitTime = 0f;

    private Coroutine gameTimerCoroutine;

    private void Start()
    {
        _endGame.enabled = false;
        _photonView = GetComponent<PhotonView>();
        StartGameTimer();
        _photonView.RPC("RestartGame", RpcTarget.All);
    }

    private void Update()
    {
        if (!isGameend)
        {
            elapsedTime += Time.deltaTime;
            waitTime += Time.deltaTime;
            if (elapsedTime >= 3f)
            {
                elapsedTime = 0f;

                CheckForRoundEnd();
                if (!isRoundRestarting)
                {
                    _photonView.RPC("CheckForGameEnd", RpcTarget.All);

                }
            }
        }
    }

    private void Awake()
    {
        SetConnection();
    }

    private void StartGameTimer()
    {
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }
        gameTimerCoroutine = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (_timeSecond > 0)
        {
            yield return new WaitForSeconds(1);
            _timeSecond--;
            _photonView.RPC("UpdateTimeText", RpcTarget.All, _timeSecond);
        }

        if (_timeSecond <= 0 && !isRoundRestarting)
        {
            isRoundRestarting = true;
            _photonView.RPC("RestartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void UpdateTimeText(int time)
    {
        _time.text = $"Time: {time}";
    }

    [PunRPC]
    private void RestartGame()
    {
        _photonView.RPC("DeleteAllPlayersRPC", RpcTarget.All);
        _photonView.RPC("Respawn", RpcTarget.All);
        _timeSecond = 45;
        isRoundRestarting = false;
        StartGameTimer();
        _photonView.RPC("UpdateTimeText", RpcTarget.All, _timeSecond);
    }

    [PunRPC]
    private void DeleteAllPlayersRPC()
    {
        DeleteAllPlayersTRPC();
        DeleteAllPlayersCTRPC();
    }

    [PunRPC]
    private void DeleteAllPlayersTRPC()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Terrorist"))
            {
                PhotonNetwork.Destroy(obj);
            }
        }
    }

    [PunRPC]
    private void Respawn()
    {
        if (!isGameend)
        {
            PlayerManager.tagSet = false;
        }
    }

    [PunRPC]
    private void DeleteAllPlayersCTRPC()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("CounterT"))
            {
                PhotonNetwork.Destroy(obj);
            }
        }
    }

    private void CheckForRoundEnd()
    {

        if (!isGameend)
        {
            if (isRoundRestarting)
                return;

            int playerTCount = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("Terrorist")).Count();
            int playerCTCount = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name.Contains("CounterT")).Count();
            Debug.LogWarning("T: " + playerTCount);
            Debug.LogWarning("CT: " + playerCTCount);

            if (playerCTCount == 0 && playerTCount == 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            else if (playerTCount == 0 || playerCTCount == 0)
            {
                if (playerTCount == 0)
                {
                    _scoreCTValue++;
                    TrueOrFalse(1, MyLogin.login, "CT");
                    Score(_randomRating, MyLogin.login, "CT");
                }
                else if (playerCTCount == 0)
                {
                    _scoreTValue++;
                    TrueOrFalse(1, MyLogin.login, "T");
                    Score(_randomRating, MyLogin.login, "T");
                }

                _photonView.RPC("UpdateScoreText", RpcTarget.All, _scoreCTValue, _scoreTValue);
                _photonView.RPC("RestartGame", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void UpdateScoreText(int scoreCT, int scoreT)
    {
        _scoreCT.text = $"CT: {scoreCT}";
        _scoreT.text = $"T: {scoreT}";
    }

    [PunRPC]
    private void CheckForGameEnd()
    {
        if (_scoreCTValue >= 6 || _scoreTValue >= 6)
        {
            _endGame.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isGameend = true;
            _photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void EndGame()
    {
        _time.enabled = false;
        _scoreT.enabled = false;
        _scoreCT.enabled = false;

        if (_photonView.IsMine)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (_photonView.IsMine)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LeaveRoom();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(2);
        if (_photonView.IsMine)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(4);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Disconnected()
    {
        DeleteAllPlayerManagers();
        PhotonNetwork.Disconnect();
        StartCoroutine(DelayedLoadMainMenu());
    }

    private IEnumerator DelayedLoadMainMenu()
    {
        yield return new WaitForSeconds(5);
        if (_photonView.IsMine)
        {
            PhotonNetwork.LoadLevel(4);
        }
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.Log("Disconnected from server: " + cause.ToString());
        StartCoroutine(ReturnToMainMenu());
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

    private async void TrueOrFalse(int counter, string login, string team)
    {

        string updateQuery = "UPDATE lal SET roundswin = roundswin + @roundswin WHERE login = @login";

        using (SqliteCommand cmd = new SqliteCommand(updateQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@roundswin", counter);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Debug.Log($"Column roundswin successfully updated for team {team}.");
                _photonView.RPC("SyncDatabaseUpdate", RpcTarget.All, login, counter);
            }
            catch (Exception e)
            {
                Debug.LogError("Update failed: " + e.Message);
            }
        }
    }
    private async void Score(int rating, string login, string team)
    {
        string updateQuery = "UPDATE las SET score = score + @score WHERE login = @login";

        using (SqliteCommand cmd = new SqliteCommand(updateQuery, dbConnection))
        {
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@score", rating);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                Debug.Log($"Column roundswin successfully updated for team {team}.");
                _photonView.RPC("SyncDatabaseUpdate", RpcTarget.All, login, rating);
            }
            catch (Exception e)
            {
                Debug.LogError("Update failed: " + e.Message);
            }
        }
    }

    [PunRPC]
    private void SyncDatabaseUpdate(string login, int counter)
    {
        Debug.Log($"Syncing database update: {login}, {counter}");
    }

    [PunRPC]
    private void DeleteAllPlayerManagers()
    {
        PlayerManager[] allPlayerManagers = GameObject.FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager manager in allPlayerManagers)
        {
            PhotonNetwork.Destroy(manager.gameObject);
        }
    }
}