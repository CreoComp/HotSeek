using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class RestartGame: MonoBehaviour
{
    public GameObject panelWinner;
    public TextMeshProUGUI textWinner;

    [PunRPC]
    public void Restart(string winnerNickname) 
    {
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