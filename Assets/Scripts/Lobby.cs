using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    private string roomName;
    [SerializeField] private InputField input;
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnChangeNameRoom()
    {
        roomName = input.text;
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("<color=green>Connected</color> to room " + roomName);
    }
}
