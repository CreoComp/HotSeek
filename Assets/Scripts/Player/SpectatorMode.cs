using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpectatorMode: MonoBehaviour
{
    private List<PhotonView> players = new List<PhotonView>();
    private int playerIndex;
    private PhotonView view;
    private bool isSpectator;

    public void SetMode()
    {
        isSpectator = true;
        view = gameObject.AddComponent<PhotonView>();

        players = FindObjectsOfType<PhotonView>().ToList();
        players.Remove(view);

        SetSpectateToPlayer(players[playerIndex]);
    }

    private void Update()
    {
        if (!isSpectator)
            return;

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
        Debug.Log("set spectate");
    }
}
