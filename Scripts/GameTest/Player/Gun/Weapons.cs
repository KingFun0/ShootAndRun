using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Weapons : MonoBehaviour
{
    public int damage;
    public Camera camera;
    public float fireRate;



    private float nextFire;

    [Header("Ammo")]
    public int mag = 3;

    public int ammo = 25;
    public int magAmmo = 25;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("SFX")] public int shootSFXindex = 0;
    public PlayerPhotonSoundManager soundManager;

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && nextFire <= 0 && ammo > 0)
        {
            nextFire = 1 / fireRate;
            ammo--;

            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reaload();
        }
    }

    void Reaload()
    {
        if(mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
    }


    void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        soundManager.PlayShootSFX(shootSFXindex);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {

            Healths targetHealth = hit.transform.gameObject.GetComponent<Healths>();
            if (targetHealth != null)
            {
                PhotonView targetPhotonView = hit.transform.gameObject.GetComponent<PhotonView>();
                if (targetPhotonView != null && !IsSameTeam(targetPhotonView))
                {
                    targetPhotonView.RPC("TakeDamage", RpcTarget.All, damage);
                }
            }
        }
    }

    bool IsSameTeam(PhotonView targetPhotonView)
    {
        string myTag = this.gameObject.tag;
        string targetTag = targetPhotonView.gameObject.tag;

        return myTag == targetTag;
    }
}
