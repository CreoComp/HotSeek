using Photon.Pun;
using UnityEngine;

public class IsMine : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PhotonView view;
    [SerializeField] private GameObject cam;

    private void Awake()
    {
        if (!view.IsMine)
        {
            movement.enabled = false;
            cam.SetActive(false);
        }
    }
}

