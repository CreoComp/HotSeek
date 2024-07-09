using UnityEngine;
using TMPro; 

public class GameEndScreen : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    void Start()
    {
        winnerText = GetComponent<TextMeshProUGUI>();
    }

    public void ShowWinner(string winnerName)
    {
        winnerText.text = "Победитель: " + winnerName;
    }
}

