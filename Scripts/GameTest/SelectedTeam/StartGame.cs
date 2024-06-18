using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class StartGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<TMP_Text> nickNamesT = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> nickNamesCT = new List<TMP_Text>();
    [SerializeField] private Canvas _selectedTeams;
    [SerializeField] private Canvas _gameCanvas;

    static public bool isGameStart = false;

    private void Update()
    {
        if (AreAllPlayersReady())
        {
            StartGameForAllPlayers();
        }
    }

    private bool AreAllPlayersReady()
    {
        foreach (var name in nickNamesT)
        {
            if (string.IsNullOrEmpty(name.text))
            {
                return false;
            }
        }
        foreach (var name in nickNamesCT)
        {
            if (string.IsNullOrEmpty(name.text))
            {
                return false;
            }
        }
        return true;
    }

    private void StartGameForAllPlayers()
    {
        photonView.RPC("RPC_StartGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        _selectedTeams.gameObject.SetActive(false);
        _gameCanvas.gameObject.SetActive(true);
        isGameStart = true;
    }

 
}
