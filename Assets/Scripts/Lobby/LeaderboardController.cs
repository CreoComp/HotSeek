using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    private bool isLeaderboardVisible = false;

    private void Start()
    {
        leaderboardPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            isLeaderboardVisible = !isLeaderboardVisible;
            leaderboardPanel.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab)) 
        {
            leaderboardPanel.SetActive(false);
        }
    }
}
