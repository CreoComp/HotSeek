using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    private GameObject[] boosters;

    private GameObject activeBooster;
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
            if(activeBooster == null)
            activeBooster = PhotonNetwork.Instantiate(boosters[Random.Range(0, boosters.Length)].name, transform.position, Quaternion.identity);
        }
    }
}
