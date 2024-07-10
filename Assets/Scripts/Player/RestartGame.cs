using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class RestartGame: MonoBehaviour
{
    public GameObject panelCatch;
    public GameObject panelWinner;
    public TextMeshProUGUI textWinner;
    private PhotonView view;
    private PlayerTimerText playerTimerText;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        playerTimerText = FindObjectOfType<PlayerTimerText>();
    }

    [PunRPC]
    public void WinRPC(int ActorID)
    {
        Debug.Log("restart");
        Restart(FindPhotonViewByControllerActorNr(ActorID).Owner.NickName);
    }

    public void Restart(string winnerNickname) 
    {
        panelCatch.SetActive(false);
        panelWinner.SetActive(true);
        textWinner.text = "Победитель\n" + winnerNickname;
        StartCoroutine(RestartTimer());
    }

    IEnumerator RestartTimer()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LoadLevel("Game");
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