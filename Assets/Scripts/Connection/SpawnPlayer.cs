using Photon.Pun;
using UnityEngine;

public class SpawnPlayer: MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform spawnPosition;

    private void Start()
    {
        PhotonNetwork.Instantiate(_player.name, spawnPosition.position, Quaternion.identity);
    }
}
