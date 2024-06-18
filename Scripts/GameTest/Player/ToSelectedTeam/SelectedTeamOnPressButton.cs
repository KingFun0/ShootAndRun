using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class SelectedTeamOnPressButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button _buttonT;
    [SerializeField] private Button _buttonCT;

    [SerializeField] private List<TMP_Text> nickNamesT = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> nickNamesCT = new List<TMP_Text>();

    private int maxTeamSize = 1;
    private int countT = 0;
    private int countCT = 0;
    private bool hasChosenTeam = false;
    static public bool isAllPlayersSelectedTeam = false;
    private string playerName = MyLogin.login;

    private void Start()
    {
        _buttonT.onClick.AddListener(OnTeamTButtonClick);
        _buttonCT.onClick.AddListener(OnTeamCTButtonClick);
        isAllPlayersSelectedTeam = false;
    }

    private void OnTeamTButtonClick()
    {
        if (!hasChosenTeam)
        {
            photonView.RPC("RPC_AddInTeamT", RpcTarget.AllBuffered, playerName, "Player_T");
        }
    }

    private void OnTeamCTButtonClick()
    {
        if (!hasChosenTeam)
        {
            photonView.RPC("RPC_AddInTeamCT", RpcTarget.AllBuffered, playerName, "Player_CT");
        }
    }

    [PunRPC]
    public void RPC_AddInTeamT(string playerName, string tag)
    {
        if (countT < maxTeamSize)
        {
            nickNamesT[countT].text = playerName;
            countT++;
            AssignTagToPlayer(playerName, tag);
            if (PhotonNetwork.LocalPlayer.NickName == playerName)
            {
                hasChosenTeam = true;
                _buttonT.interactable = false;
                _buttonCT.interactable = false;
            }
        }

        CheckIfAllPlayersSelectedTeam();
    }

    [PunRPC]
    public void RPC_AddInTeamCT(string playerName, string tag)
    {
        if (countCT < maxTeamSize)
        {
            nickNamesCT[countCT].text = playerName;
            countCT++;
            AssignTagToPlayer(playerName, tag);
            if (PhotonNetwork.LocalPlayer.NickName == playerName)
            {
                hasChosenTeam = true;
                _buttonT.interactable = false;
                _buttonCT.interactable = false;
            }
        }

        CheckIfAllPlayersSelectedTeam();
    }

    private void CheckIfAllPlayersSelectedTeam()
    {
        isAllPlayersSelectedTeam = (countT >= maxTeamSize && countCT >= maxTeamSize);
        Debug.LogWarning(isAllPlayersSelectedTeam);

        if (isAllPlayersSelectedTeam)
        {
            StartCoroutine(DelayedAction(5.0f));
        }
    }

    IEnumerator DelayedAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // Ваш код для выполнения действия после задержки...
    }

    private void AssignTagToPlayer(string playerName, string tag)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.Owner.NickName == playerName)
            {
                player.tag = tag;
                break;
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RemovePlayerFromTeam(otherPlayer.NickName);
    }

    private void RemovePlayerFromTeam(string playerName)
    {
        for (int i = 0; i < countT; i++)
        {
            if (nickNamesT[i].text == playerName)
            {
                nickNamesT[i].text = "";
                countT--;
                ReorderTeam(nickNamesT, countT);
                break;
            }
        }

        for (int i = 0; i < countCT; i++)
        {
            if (nickNamesCT[i].text == playerName)
            {
                nickNamesCT[i].text = "";
                countCT--;
                ReorderTeam(nickNamesCT, countCT);
                break;
            }
        }

        isAllPlayersSelectedTeam = false;
    }

    private void ReorderTeam(List<TMP_Text> nickNames, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (string.IsNullOrEmpty(nickNames[i].text))
            {
                for (int j = i + 1; j < nickNames.Count; j++)
                {
                    if (!string.IsNullOrEmpty(nickNames[j].text))
                    {
                        nickNames[i].text = nickNames[j].text;
                        nickNames[j].text = "";
                        break;
                    }
                }
            }
        }
    }
}
