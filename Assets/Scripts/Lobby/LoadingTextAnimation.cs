using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingTextAnimation : MonoBehaviour
{
    private TextMeshProUGUI TextArea;// ���� ������ ������

    private string Text;// ���. ���������� ��� �������� �� �����
    private void Start()
    {
        TextArea = GetComponent<TextMeshProUGUI>();
        TextArea.text = "";
        StartCoroutine(TextAnimation());
    }
    private IEnumerator TextAnimation()
    {
        while (true)
        {
            TextArea.text = "Loading";
            yield return new WaitForSeconds(0.1f);
            TextArea.text = "Loading.";
            yield return new WaitForSeconds(0.1f);
            TextArea.text = "Loading..";
            yield return new WaitForSeconds(0.1f);
            TextArea.text = "Loading...";
            yield return new WaitForSeconds(0.1f);
        }
    }
}


