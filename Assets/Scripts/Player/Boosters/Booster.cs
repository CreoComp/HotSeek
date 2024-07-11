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

    private float attackForce = 15f;

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

    [PunRPC]
    public void PushMeAway(int ActorID, int x, int y, int z)
    {
        if(ActorID == view.ControllerActorNr && view.IsMine)
        StartCoroutine(Push(new Vector3(x,y,z)));
    }

    public IEnumerator Push(Vector3 playerAttacking)
    {
        for (int i = 0; i < 100; i++)
        {
            controller.Move((transform.position - playerAttacking) * attackForce);
            yield return new WaitForSeconds(1f / 100f);
        }
    }
}
