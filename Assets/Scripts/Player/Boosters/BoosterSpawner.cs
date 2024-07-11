using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    private GameObject[] boosters;
    void Start()
    {
        boosters = Resources.LoadAll<GameObject>("Boosters");

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnRandomBooster());
    }

    IEnumerator SpawnRandomBooster()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(15, 30));
            PhotonNetwork.Instantiate(boosters[Random.Range(0, boosters.Length)].name, transform.position, Quaternion.identity);
        }
    }
}
