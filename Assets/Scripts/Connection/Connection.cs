using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Connection : MonoBehaviourPunCallbacks
{
    private string SceneName = "Lobby";
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("<color=green>Connected</color>");
        SceneManager.LoadScene(SceneName);
    }
}
