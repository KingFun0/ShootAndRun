using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    private PhotonView _photonView;
    private string playerTag;
    static public float x, y, z;
    static public bool tagSet = false;

    void Start()
    {
        _photonView = GetComponent<PhotonView>();

        // Set the tag for all PlayerManager instances to "Player"
        PlayerManager[] allPlayers = FindObjectsOfType<PlayerManager>();
        foreach (PlayerManager player in allPlayers)
        {
            player.tag = "Player";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("TCount"))
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "TCount", 0 } });
            }
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("CTCount"))
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CTCount", 0 } });
            }
        }
    }

    private void Update()
    {

            Spawn();
        
    }
    private void Spawn()
    {
        playerTag = this.tag;
        if (_photonView.IsMine && !tagSet && StartGame.isGameStart)
        {


            if (playerTag == "Player_T")
            {
                int tCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["TCount"];
                if (tCount == 0)
                {
                    SetUpPlayerFirstT();
                }
                else if (tCount == 1)
                {
                    SetUpPlayerSecondT();
                }
                GameObject tPlayer = CreateT(x, y, z);
                tPlayer.tag = "Player_T";
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "TCount", tCount + 1 } });
                tagSet = true;
            }
            else if (playerTag == "Player_CT")
            {
                int ctCount = (int)PhotonNetwork.CurrentRoom.CustomProperties["CTCount"];
                if (ctCount == 0)
                {
                    SetUpPlayerFirstCT();
                }
                else if (ctCount == 1)
                {
                    SetUpPlayerSecondCT();
                }
                GameObject ctPlayer = CreateCT(x, y, z);
                ctPlayer.tag = "Player_CT";
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "CTCount", ctCount + 1 } });
                tagSet = true;
            }
        }
    }
    private void SetUpPlayerFirstCT()
    {
        x = -149.55f;
        y = -0.09f;
        z = 21.26f;
    }

    private void SetUpPlayerSecondCT()
    {
        x = -144.66f;
        y = -0.09f;
        z = 12.86f;
    }

    private void SetUpPlayerFirstT()
    {
        x = -190.94f;
        y = -0.09f;
        z = 42.33f;
    }

    private void SetUpPlayerSecondT()
    {
        x = -184.34f;
        y = -0.09f;
        z = 45.47f;
    }

    private GameObject CreateCT(float x, float y, float z)
    {
        return PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "CounterT"), new Vector3(x, y, z), Quaternion.identity);
    }

    private GameObject CreateT(float x, float y, float z)
    {
        return PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Terrorist"), new Vector3(x, y, z), Quaternion.identity);
    }
}
