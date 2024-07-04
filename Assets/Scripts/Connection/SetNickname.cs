using Photon.Pun;
using TMPro;
using UnityEngine;

public class SetNickname : MonoBehaviour
{
    public TMP_InputField input;
    private string nick;

    public void OnChangedNick()
    {
        nick = input.text;
        PhotonNetwork.NickName = nick;
    }
}
