using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerNickNames : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _playerNickName;

    private void Start()
    {
        PhotonNetwork.NickName = MyLogin.login;
        _playerNickName.text = MyLogin.login;
    }

}
