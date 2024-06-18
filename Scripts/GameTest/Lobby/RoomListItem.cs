using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    private RoomInfo _info;
    public void SetUp(RoomInfo roomInfo)
    {
        _info = roomInfo;
        roomName.text = _info.Name;
    }

    public void OnClick()
    {
       ConnectToServer.instance.JoinRoom(_info);
    }
}
