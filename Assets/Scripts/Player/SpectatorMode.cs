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


    public void SetMode(PhotonView _view)
    {
        isSpectator = true;
        view = _view;

        players = FindObjectsOfType<PhotonView>().ToList();
        players.Remove(view);

        for(int i = 0; i < players.Count; i++) 
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
            if (players[i].GetComponent<CatchPlayer>() == null)
            {
                players.RemoveAt(i);
                i--;
            }
        }

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
