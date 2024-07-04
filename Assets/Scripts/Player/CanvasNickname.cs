using Photon.Pun;
using TMPro;
using UnityEngine;

public class CanvasNickname : MonoBehaviour
{
    private Camera camera;
    public TextMeshProUGUI textNick;
    private void Start()
    {
        camera = FindObjectOfType<Camera>();
        GetComponent<Canvas>().worldCamera = camera;
        textNick.text = transform.parent.GetComponent<PhotonView>().Owner.NickName;
    }

    private void Update()
    {
        Vector3 direction = camera.gameObject.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }
}
