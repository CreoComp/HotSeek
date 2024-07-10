using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingTextAnimation : MonoBehaviour
{
    private TextMeshProUGUI TextArea;// поле вывода текста

    private string Text;// доп. переменная для проверки на знаки
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


