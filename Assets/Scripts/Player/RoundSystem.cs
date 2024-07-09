﻿using Photon.Pun;
using UnityEngine;

public class RoundSystem: MonoBehaviour
{
    public PlayerTimerText playerTimerText;
    private PhotonView view;

    private void Start()
    {
        playerTimerText = GetComponent<PlayerTimerText>();
        view = GetComponent<PhotonView>();

        if (view.AmOwner)
            gameObject.AddComponent<OwnerRoundSystem>();


        view.RPC("AddNewPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
    }

    private void OnDestroy()
    {
        view.RPC("DefeatPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
    }

    [PunRPC]
    public void TimerToStart()
    {
        playerTimerText.StartTimer();
    }

    [PunRPC]
    public void End()
    {
        playerTimerText.StopTimer();
    }

    [PunRPC]
    public void FindBoomPlayer(int ActorId)
    {
        PhotonView playerView = FindPhotonViewByControllerActorNr(ActorId);
        playerView.GetComponent<RoundSystem>().Boom();
    }

    public void Boom()
    {
        view.RPC("DefeatPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
        PhotonNetwork.LeaveLobby(); // сделать наблюдателя
    }

    PhotonView FindPhotonViewByControllerActorNr(int actorNumber)
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in photonViews)
        {
            if (view.ControllerActorNr == actorNumber)
            {
                return view;
            }
        }
        return null;
    }

    
}
