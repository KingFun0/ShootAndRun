using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Healths : MonoBehaviour
{
    public int health;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public Canvas playerCanvas;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        // Скрываем Canvas у всех игроков, кроме локального
        if (!_photonView.IsMine)
        {
            playerCanvas.enabled = false;
        }
        else
        {
            playerCanvas.enabled = true;
        }
    }

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        health -= _damage;

        if (_photonView.IsMine)
        {
            healthText.text = health.ToString();
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
