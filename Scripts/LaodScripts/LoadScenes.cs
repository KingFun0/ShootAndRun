using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class LoadScenes : MonoBehaviour
{
    public void SceneNew(int id)
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        Load.SetSceneID(id);
    }
    public void Loaded(int level)
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(level);
    }
}
