using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosterGun : MonoBehaviour, IBoostable
{
    private string nameBooster = "Дробовик";
    private Booster boosterActivator;
    private Transform rayStart;
    public float coneAngle = 90f; // Угол конуса (в градусах)
    public float detectionDistance = 10f;

    public void Construct(Booster boosterActivator, CharacterController controller, Transform _rayStart)
    {
        this.boosterActivator = boosterActivator;
        this.rayStart = _rayStart;
    }

    public void Boost()
    {
        List<CharacterController> players = FindObjectsOfType<CharacterController>().ToList();

        foreach(CharacterController player in players)
        {
            if(IsTargetInCone(player.transform))
            {
                if (Physics.Raycast(rayStart.position, player.transform.position, detectionDistance))
                {
                    PushAway(player.transform);
                }
            }
        }
        
        boosterActivator.UseBooster();
        DestroyComponent();
    }

    public void PushAway(Transform target)
    {
        GetComponent<PhotonView>().RPC("PushMeAway", RpcTarget.AllBuffered, transform.position.x, transform.position.y, transform.position.z);
    }

    bool IsTargetInCone(Transform target)
    {
        Vector3 directionToTarget = target.position - rayStart.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > detectionDistance)
        {
            return false; // Объект слишком далеко
        }

        directionToTarget.Normalize();
        float angleToTarget = Vector3.Angle(rayStart.forward, directionToTarget);

        if (angleToTarget < coneAngle / 2)
        {
            return true; // Объект внутри конуса
        }

        return false; // Объект вне конуса
    }
    public string NameBooster() =>
    nameBooster;
    public void DestroyComponent()
    {
        Destroy(this);
    }
}