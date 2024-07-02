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

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = new Ray(rayStartPosition.position, rayStartPosition.forward * distance);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.TryGetComponent(out CatchPlayer player))
                {
                    view.RPC("SetPotatoToPlayer", RpcTarget.AllBuffered, player);
                    view.RPC("UnSetPotatoFromPlayer", RpcTarget.AllBuffered, this);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            view.RPC("SetPotatoToPlayer", RpcTarget.All, this);
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
    }

    public void UnSetHotPotato()
    {
        isHotPotato = true;
        textCatch.text = "БЕГИИИИ .... БЕГИИИИ";
    }

    [PunRPC]
    public void SetPotatoToPlayer(CatchPlayer player)
    {
        if(player == this)
        {
            SetHotPotato();
        }
    }

    [PunRPC]
    public void UnSetPotatoFromPlayer(CatchPlayer player)
    {
        if (player == this)
        {
            UnSetHotPotato();
        }
    }
}