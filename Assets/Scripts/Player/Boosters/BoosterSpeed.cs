using System.Collections;
using UnityEngine;

public class BoosterSpeed : MonoBehaviour, IBoostable
{
    private string nameBooster = "+Скорость";
    private Booster boosterActivator;
    private PlayerMovement movement;
    private float speedMultiplier = 2f;
    private float timeBooster = 6f;


    public void Construct(Booster boosterActivator, PlayerMovement playerMovement)
    {
        this.boosterActivator = boosterActivator;
        this.movement = playerMovement;
    }

    public void Boost()
    {
        boosterActivator.UseBooster();
        StartCoroutine(Speed());
    }

    IEnumerator Speed()
    {
        movement.SpeedMultiplier = speedMultiplier;
        yield return new WaitForSeconds(timeBooster);

        DestroyComponent();
    }
    public string NameBooster() =>
    nameBooster;
    public void DestroyComponent()
    {
        movement.SpeedMultiplier = 1f;
        Destroy(this);
    }
}
