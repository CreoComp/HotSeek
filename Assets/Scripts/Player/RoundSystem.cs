using Photon.Pun;
using UnityEngine;

public class RoundSystem: MonoBehaviour
{
    private PlayerTimerText playerTimerText;
    private PhotonView view;

    private void Start()
    {
        playerTimerText = FindObjectOfType<PlayerTimerText>();
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
    public void AddNewPlayer(int ActorId)
    {
        if(view.AmOwner)
        {
            GetComponent<OwnerRoundSystem>().AddNewPlayer(ActorId);
        }
    }

    [PunRPC]
    public void DefeatPlayer(int ActorId)
    {
        if (view.AmOwner)
        {
            GetComponent<OwnerRoundSystem>().DefeatPlayer(ActorId);
        }
    }

    [PunRPC]
    public void TimerToStart()
    {
        playerTimerText.StartTimer();
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
        //PhotonNetwork.LeaveLobby();
        // сделать наблюдателя
        SetSpectator();
    }

    public void SetSpectator()
    {
        Camera.main.GetComponent<SpectatorMode>().SetMode();
        Destroy(gameObject);
    }

    [PunRPC]
    public void Win(int ActorID)
    {
        if(ActorID == view.ControllerActorNr)
        {
            view.RPC("Restart", RpcTarget.AllBuffered, PhotonNetwork.NickName);
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
}
