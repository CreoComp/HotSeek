using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Booster : MonoBehaviour
{
    private IBoostable activeBooster;
    public Transform rayStart;
    private CharacterController controller;
    private PhotonView view;
    private GameObject panelTextActiveBooster;
    private TextMeshProUGUI textActiveBooster;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        view = GetComponent<PhotonView>();

        panelTextActiveBooster = FindObjectOfType<PanelActiveBooster>().gameObject;
        textActiveBooster = panelTextActiveBooster.GetComponentInChildren<TextMeshProUGUI>();
        RemoveText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && activeBooster != null)
            activeBooster.Boost();
    }

    public void SetText()
    {
        panelTextActiveBooster.SetActive(true);
        textActiveBooster.text = "Доступный бустер\n" + "<size=40><color=green>" + activeBooster.NameBooster();
    }

    public void RemoveText()
    {
        panelTextActiveBooster.SetActive(false);
    }

    public void UseBooster()
    {
        activeBooster = null;
        RemoveText();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PickableBooster boosterTrigger))
        {
            if(activeBooster != null)
            {
                activeBooster.DestroyComponent();
            }
            switch(boosterTrigger.Type)
            {
                case BoosterType.Speed:
                    var booster1 = gameObject.AddComponent<BoosterSpeed>();
                    booster1.Construct(this, GetComponent<PlayerMovement>());
                    break;

                case BoosterType.Jump:
                    var booster2 = gameObject.AddComponent<BoosterJump>();
                    booster2.Construct(this, GetComponent<PlayerMovement>());
                    break;

                case BoosterType.Teleport:
                    var booster3 = gameObject.AddComponent<BoosterTeleport>();
                    booster3.Construct(this, controller, rayStart);
                    break;

                case BoosterType.Gun:
                    var booster4 = gameObject.AddComponent<BoosterGun>();
                    booster4.Construct(this, controller, rayStart);
                    break;

                case BoosterType.GlobalVision:
                    var booster5 = gameObject.AddComponent<BoosterGlobalVision>();
                    booster5.Construct(this);
                    break;
            }
            activeBooster = GetComponent<IBoostable>();
            Destroy(other.gameObject);
            SetText();
        }
    }
}
