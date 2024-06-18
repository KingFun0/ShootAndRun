using Photon.Pun;
using UnityEngine;

public class PlayerTag : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine)
        {
           
        }
    }
    public void SetPlayerTag(string tag)
    {
        gameObject.tag = tag;
    }
}
