using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    private string[] boosters = new string[]
    {
        "Speed", "Jump", "Teleport", "Gun", "Vision"
    };

    private GameObject activeBooster;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnRandomBooster());
    }

    IEnumerator SpawnRandomBooster()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(15, 30));
            if(activeBooster == null)
            activeBooster = PhotonNetwork.Instantiate(boosters[Random.Range(0, boosters.Length)], transform.position, Quaternion.identity);
        }
    }
}
