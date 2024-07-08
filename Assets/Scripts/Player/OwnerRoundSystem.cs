using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class OwnerRoundSystem: MonoBehaviourPunCallbacks
{
    public GameObject startButton;
    private PhotonView view;

    private List<PhotonView> players = new List<PhotonView>();

    void Awake()
    {
        view = GetComponent<PhotonView>();
        startButton.SetActive(true);
    }

    [PunRPC] 
    public void AddNewPlayer(int ActorId)
    {
        players.Add(FindPhotonViewByControllerActorNr(ActorId));
    }

    [PunRPC]
    public void DisconnectPlayer(int ActorId)
    {
        players.Remove(FindPhotonViewByControllerActorNr(ActorId));
    }

    public void StartGameButton()
    {
        view.RPC("TimerToStart", RpcTarget.AllBuffered);
        startButton.SetActive(false);
    }
    public void SetRandomPlayerHot()
    {
        

        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        int randomActorId = photonViews[UnityEngine.Random.Range(0, photonViews.Length)].ControllerActorNr;
        view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, randomActorId);
    }

    public void TimeToBoomPlayer()
    {
        view.RPC("FindBoomPlayer", RpcTarget.AllBuffered);
        SetRandomPlayerHot();
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

    PhotonView FindPhotonViewByPotato()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView view in photonViews)
        {
            if (view.GetComponent<CatchPlayer>().IsHotPotato)
            {
                return view;
            }
        }
        return null;
    }
}