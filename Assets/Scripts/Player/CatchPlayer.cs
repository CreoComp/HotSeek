using TMPro;
using UnityEngine;
using Photon;
using Photon.Pun;

public class CatchPlayer: MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI textCatch;
    private bool isHotPotato;
    public Transform rayStartPosition;
    public float distance = 5f;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!view.IsMine)
            return;

        if (Input.GetMouseButtonDown(0) && isHotPotato)
        {
            RaycastHit hit;
            Ray ray = new Ray(rayStartPosition.position, rayStartPosition.forward * distance);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.TryGetComponent(out CatchPlayer player))
                {
                    view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ControllerActorNr);
                    view.RPC("UnSetPotatoFromPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, view.ControllerActorNr);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, 1);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, 2);
        }
    }

    private void OnGUI()
    {
        Debug.DrawRay(rayStartPosition.position, rayStartPosition.forward * distance);
    }

    public void SetHotPotato()
    {
        isHotPotato = true;
        textCatch.text = "У ВАС КАРТОХА!!!1Й1!";
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void UnSetHotPotato()
    {
        isHotPotato = true;
        textCatch.text = "БЕГИИИИ .... БЕГИИИИ";
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [PunRPC]
    public void SetPotatoToPlayer(int ActorId)
    {
        Debug.Log("Set potato to new player " + ActorId);
        if(view.ControllerActorNr == ActorId)
        {
            Debug.Log("this player with id " + ActorId);
            SetHotPotato();
        }
    }

    [PunRPC]
    public void UnSetPotatoFromPlayer(int ActorId)
    {
        if (view.ControllerActorNr == ActorId)
        {
            UnSetHotPotato();
        }
    }
}