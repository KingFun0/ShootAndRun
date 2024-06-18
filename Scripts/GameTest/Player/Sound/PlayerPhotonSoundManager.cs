using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerPhotonSoundManager : MonoBehaviour
{
    public AudioSource footstepSource;

    public AudioClip footstepSFX;

    public AudioSource gunShootSorce;
    public AudioClip[] allGunShootsSFX; 
    public void PlayerFootstepSFX()
    {
        GetComponent<PhotonView>().RPC("PlayerFootStepSFX_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayerFootStepSFX_RPC()
    {
        footstepSource.clip = footstepSFX;

        //Звук и громкость
        footstepSource.pitch = UnityEngine.Random.RandomRange(0.7f, 1.2f);
        footstepSource.volume = UnityEngine.Random.RandomRange(0.2f, 0.35f);


        footstepSource.Play();
    }

    public void PlayShootSFX(int index)
    {
        GetComponent<PhotonView>().RPC("PlayShootSFX_RPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void PlayShootSFX_RPC(int index)
    {
        gunShootSorce.clip = allGunShootsSFX[index];

        //Звук и громкость
        gunShootSorce.pitch = UnityEngine.Random.RandomRange(0.7f, 1.2f);
        gunShootSorce.volume = UnityEngine.Random.RandomRange(0.2f, 0.35f);


        gunShootSorce.Play();
    }
}
