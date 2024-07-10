using Photon.Pun;
using UnityEngine;

public class IsMine : MonoBehaviour
{
    private PlayerMovement movement;
    private PhotonView view;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            movement.enabled = false;
        }
    }
}

