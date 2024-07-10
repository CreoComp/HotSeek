using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectatorMode: MonoBehaviour
{
    private PhotonView playerSpectate;
    private List<PhotonView> players = new List<PhotonView>();
    private int playerIndex;

    private PhotonView view;
    private void Awake()
    {
        view = gameObject.AddComponent<PhotonView>();

        players = FindObjectsOfType<PhotonView>().ToList();
        players.Remove(view);

        SetSpectateToPlayer(players[playerIndex]);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
            PreviousPlayer();
        if (Input.GetMouseButtonDown(1))
            NextPlayer();
    }

    public void NextPlayer()
    {
        playerIndex++;
        if (playerIndex > players.Count - 1)
            playerIndex = 0;

        SetSpectateToPlayer(players[playerIndex]);
    }

    public void PreviousPlayer()
    {
        playerIndex--;
        if (playerIndex < 0)
            playerIndex = players.Count - 1;

        SetSpectateToPlayer(players[playerIndex]);
    }

    public void SetSpectateToPlayer(PhotonView view)
    {
        GetComponent<PlayerCamera>().target = view.transform;
    }
}

public class RestartGame: MonoBehaviour
{
    [PunRPC]
    public void Restart() 
    {

    }
}