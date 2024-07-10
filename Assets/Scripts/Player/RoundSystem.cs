using Photon.Pun;
using UnityEngine;

public class RoundSystem: MonoBehaviour
{
    private PlayerTimerText playerTimerText;
    private PhotonView view;
    private OwnerRoundSystem ownerRoundSystem;

    private void Start()
    {
        playerTimerText = FindObjectOfType<PlayerTimerText>();
        view = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient && view.IsMine)
        {
            ownerRoundSystem = new GameObject().AddComponent<OwnerRoundSystem>();
            ownerRoundSystem.Construct(this, view);
        }

        if(view.IsMine)
        view.RPC("AddNewPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
    }


/*    private void OnDestroy()
    {
        view.RPC("DefeatPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
    }*/

    [PunRPC]
    public void AddNewPlayer(int ActorId)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Add");
        FindObjectOfType<OwnerRoundSystem>().AddNewPlayer(ActorId);   
    }

    [PunRPC]
    public void DefeatPlayer(int ActorId)
    {
        FindObjectOfType<OwnerRoundSystem>().DefeatPlayer(ActorId);
    }

    public void StartGameButton()
    {
        view.RPC("TimerToStart", RpcTarget.AllBuffered);
        // startButton.SetActive(false);
    }

    [PunRPC]
    public void TimerToStart()
    {
        FindMine().GetComponent<RoundSystem>().playerTimerText.StartTimer();
    }

    [PunRPC]
    public void FindBoomPlayer()
    {
        var client = FindMine().GetComponent<CatchPlayer>();
        if (client.IsHotPotato)
        client.GetComponent<RoundSystem>().Boom();
    }

    public void Boom()
    {
        if(view.IsMine)
        {
            view.RPC("DefeatPlayer", RpcTarget.MasterClient, view.ControllerActorNr);
            //PhotonNetwork.LeaveLobby();
            // сделать наблюдателя
            SetSpectator();
        }
    }

    [PunRPC]
    public void DestroyMasterClientWhenHimDefeat(int ActorID)
    {
        var master = FindPhotonViewByControllerActorNr(ActorID);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            Destroy(GetComponent<CatchPlayer>());
            Destroy(GetComponent<PlayerMovement>());

        }

    }

    public void SetSpectator()
    {
        Camera.main.GetComponent<SpectatorMode>().SetMode();
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("DestroyMasterClientWhenHimDefeat", RpcTarget.AllBuffered, view.ControllerActorNr);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }

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

    PhotonView FindMine()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView _view in photonViews)
        {
            if (_view.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                return _view;
            }
        }
        return null;
    }
}
