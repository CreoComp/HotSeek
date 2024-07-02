using Photon.Pun;
using UnityEngine;

public class SpawnPlayer: MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
       var player = PhotonNetwork.Instantiate(_player.name, spawnPosition.position, Quaternion.identity);
        PhotonNetwork.NickName = "player " + Random.Range(0, 1000);
    }
}
