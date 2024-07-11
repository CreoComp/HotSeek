using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoosterGlobalVision : MonoBehaviour, IBoostable
{
    private string nameBooster = "Просвет";
    private Booster boosterActivator;
    private List<Outline> outlines = new List<Outline>();

    private float timeBooster = 6f;

    public void Construct(Booster boosterActivator)
    {
        this.boosterActivator = boosterActivator;
    }

    public void Boost()
    {
        StartCoroutine(SetVision());
    }

    IEnumerator SetVision()
    {
       var players = FindObjectsOfType<CatchPlayer>();

        foreach (CatchPlayer player in players)
        {
            foreach (Transform child in player.transform)
            {
                if (child.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    var outline = child.AddComponent<Outline>();
                    outline.OutlineWidth = 5f;
                    outlines.Add(outline);

                    if (player.IsHotPotato)
                        outline.OutlineColor = new Color(1, 143f / 255f, 107f / 255f);
                    else
                        outline.OutlineColor = new Color(150f / 255f, 255f / 255f, 223f / 255f);
                }
            }
        }
        boosterActivator.UseBooster();


        yield return new WaitForSeconds(timeBooster);

        DestroyComponent();
    }

    public string NameBooster() =>
    nameBooster;

    public void DestroyComponent()
    {
        foreach (var outline in outlines)
        {
            Destroy(outline);
        }
        Destroy(this);
    }
}
