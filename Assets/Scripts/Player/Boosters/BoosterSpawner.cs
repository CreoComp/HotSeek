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
    public int nowIndexBooster;
    void Start()
    {
        StartCoroutine(SpawnRandomBooster());
    }

    IEnumerator SpawnRandomBooster()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(15, 30));
            if(activeBooster == null)
            activeBooster = PhotonNetwork.Instantiate(boosters[nowIndexBooster], transform.position, Quaternion.identity);
            nowIndexBooster++;
            if(nowIndexBooster > boosters.Length - 1)
            {
                nowIndexBooster = 0;
            }
        }
    }
}
