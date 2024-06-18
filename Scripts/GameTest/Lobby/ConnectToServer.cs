using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public static ConnectToServer instance;
    [SerializeField] private TMP_InputField _roomInputField;
    [SerializeField] private TMP_Text _errorText;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _roomList;
    [SerializeField] private GameObject _roomButtonPrefab;
    [SerializeField] private Transform _playerList;
    [SerializeField] private GameObject _playerNamePrefab;
    [SerializeField] private GameObject _startGameButton;

    [SerializeField] private GameObject _selectedTwoInTwoButton;
    [SerializeField] private GameObject _selectedOneInOneButton;


    [SerializeField] private Toggle _selectedTwoInTwoToggle;
    [SerializeField] private Toggle _selectedOneInOneToggle;

    private int _whichSelectedLoadGame;
    void Start()
    {
        instance = this;
        Debug.Log("Присоединился к Мастер серверу");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.instance.OpenMenu("Loading");

    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int selectedLoadGame = 0;

            if (_selectedTwoInTwoToggle.isOn)
            {
                _selectedOneInOneToggle.isOn = false;

                selectedLoadGame = 6;
            }
            else if (_selectedOneInOneToggle.isOn)
            {
                _selectedTwoInTwoToggle.isOn = false;
                selectedLoadGame = 7;
            }

            if (_whichSelectedLoadGame != selectedLoadGame)
            {
                _whichSelectedLoadGame = selectedLoadGame;
                Hashtable hash = new Hashtable();
                hash.Add("SelectedLoadGame", _whichSelectedLoadGame);
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            }
        }
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Присоединился к Мастер серверу");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Присоединился к Лобби");
        MenuManager.instance.OpenMenu("Title");
        PhotonNetwork.NickName = MyLogin.login; 
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(_roomInputField.text);
        MenuManager.instance.OpenMenu("Loading");
    }


    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(_whichSelectedLoadGame);
        }
    }


    public override void OnJoinedRoom()
    {
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("Room");
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < _playerList.childCount; i++)
        {
            Destroy(_playerList.GetChild(i).gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _selectedTwoInTwoButton.SetActive(PhotonNetwork.IsMasterClient);
        _selectedOneInOneButton.SetActive(PhotonNetwork.IsMasterClient);

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("SelectedLoadGame"))
        {
            _whichSelectedLoadGame = (int)PhotonNetwork.CurrentRoom.CustomProperties["SelectedLoadGame"];
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("SelectedLoadGame"))
        {
            _whichSelectedLoadGame = (int)propertiesThatChanged["SelectedLoadGame"];
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        _selectedTwoInTwoButton.SetActive(PhotonNetwork.IsMasterClient);
        _selectedOneInOneButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _errorText.text = "Ошибка: " + message;
        MenuManager.instance.OpenMenu("Error");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("Title");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("Loading");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < _roomList.childCount; i++)
        {
            Destroy(_roomList.GetChild(i).gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(_roomButtonPrefab, _roomList).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player player)
    {
        Instantiate(_playerNamePrefab, _playerList).GetComponent<PlayerListItem>().SetUp(player);
    }
}