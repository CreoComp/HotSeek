using Photon.Pun;
using System.Collections;
using UnityEngine;

public class RoundSystem: MonoBehaviour
{
    private PlayerTimerText playerTimerText;
    private PhotonView view;
    private OwnerRoundSystem ownerRoundSystem;
    private CharacterController controller;
    private float attackForce = 15f;


    private void Start()
    {
        playerTimerText = FindObjectOfType<PlayerTimerText>();
        view = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();

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
    public void DestroyClientWhenHimDefeat(int ActorID)
    {
        var master = FindPhotonViewByControllerActorNr(ActorID);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            Destroy(GetComponent<CatchPlayer>());
            Destroy(GetComponent<PlayerMovement>());
        }

    }

    [PunRPC]
    public void WinRPC(int ActorID)
    {
        Debug.Log("restart");
        FindObjectOfType<RestartGame>().Restart(FindPhotonViewByControllerActorNr(ActorID).Owner.NickName);
    }

    public void SetSpectator()
    {
        Camera.main.GetComponent<SpectatorMode>().SetMode(view);
        view.RPC("DestroyClientWhenHimDefeat", RpcTarget.AllBuffered, view.ControllerActorNr);
    }

    [PunRPC]
    public void PushMeAway(int ActorID, int x, int y, int z)
    {
        Debug.Log("push me" + ActorID);
        if (ActorID == view.ControllerActorNr && view.IsMine)
            StartCoroutine(Push(new Vector3(x, y, z)));
    }

    public IEnumerator Push(Vector3 playerAttacking)
    {
        for (int i = 0; i < 100; i++)
        {
            controller.Move((transform.position - playerAttacking) * attackForce);
            yield return new WaitForSeconds(1f / 100f);
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
