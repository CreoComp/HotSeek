using Photon.Pun;
using UnityEngine;

public class IsMine : MonoBehaviour
{
    private PlayerMovement movement;
    private PhotonView view;
    private RoundSystem roundSystem;
    private Booster booster;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        view = GetComponent<PhotonView>();
        roundSystem = GetComponent<RoundSystem>();
        booster = GetComponent<Booster>();
        if (!view.IsMine)
        {
            movement.enabled = false;
            roundSystem.enabled = false;
            booster.enabled = false;
        }
    }
}

