using UnityEngine;

public class BoosterTeleport : MonoBehaviour, IBoostable
{
    private string nameBooster = "Телепорт";
    private Booster boosterActivator;
    private Transform rayStart;
    private CharacterController characterController;
    private float distance = 15f;

    public void Construct(Booster boosterActivator, CharacterController controller, Transform _rayStart)
    {
        this.boosterActivator = boosterActivator;
        this.rayStart = _rayStart;
        characterController = controller;
    }

    public void Boost()
    {
        Ray ray = new Ray(rayStart.position, rayStart.forward * distance);
        if(Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            characterController.Move(hit.point - transform.position);
        }
        else
        {
            Debug.Log("non raycast");
            characterController.Move(rayStart.forward * distance);
        }
        boosterActivator.UseBooster();
        DestroyComponent();
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(rayStart.position, rayStart.forward * distance);
    }

    public string NameBooster() =>
        nameBooster;
    public void DestroyComponent()
    {
        Destroy(this);
    }
}
