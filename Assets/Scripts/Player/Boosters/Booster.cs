using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private IBoostable activeBooster;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
            activeBooster.Boost();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PickableBooster boosterTrigger))
        {
            switch(boosterTrigger.Type)
            {
                case BoosterType.Teleport:
                    gameObject.AddComponent<BoosterTeleport>();
                    break;
            }
        }
    }
}

public class BoosterTeleport: MonoBehaviour, IBoostable
{
    public void Boost()
    {
        Ray ray = new Ray(transform.position, transform.forward * 7);
        if(Physics.Raycast(ray, out RaycastHit hit, 7f))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position += (transform.forward * 7f);
        }
    }
}

public class PickableBooster: MonoBehaviour
{
    public BoosterType Type;
}

public enum BoosterType
{
    Speed,
    Jump,
    Teleport,
    GlobalVision
}
