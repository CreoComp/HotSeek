using Photon.Pun;
using UnityEngine;

public class SpawnPlayer: MonoBehaviourPunCallbacks
{
    private string _player = "Player";
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
       var player = PhotonNetwork.Instantiate(_player, spawnPosition.position, Quaternion.identity);
        PhotonNetwork.NickName = "player " + Random.Range(0, 1000);
    }
}
