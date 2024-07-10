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
        Debug.Log("player connect with id " + ActorId);
    }

    [PunRPC]
    public void DefeatPlayer(int ActorId)
    {
        players.Remove(FindPhotonViewByControllerActorNr(ActorId));
        Debug.Log("player defeat with id " + ActorId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            StartGameButton();
    }

    public void StartGameButton()
    {
        view.RPC("TimerToStart", RpcTarget.AllBuffered);
        startButton.SetActive(false);
    }
    public void SetRandomPlayerHot()
    {
        if (players.Count <= 1)
        {
            view.RPC("Win", RpcTarget.AllBuffered, players[0].ControllerActorNr);
            return;
        }

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