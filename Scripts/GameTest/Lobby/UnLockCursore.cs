using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLockCursore : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
