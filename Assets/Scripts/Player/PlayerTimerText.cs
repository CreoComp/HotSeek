using TMPro;
using UnityEngine;

public class PlayerTimerText: MonoBehaviour
{
    public GameObject panelTimer;
    public TextMeshProUGUI textTimer;
    public bool isTimerToStart;
    public bool isStarted;
    private float startTimeValue = 15f;
    private float timeBtwBoomValue = 60f;
    private float time;

    private OwnerRoundSystem ownerRoundSystem;

    private void Start()
    {
        
    }

    public void StartTimer()
    {
        isTimerToStart = true;
        time = startTimeValue;
    }

    public void StopTimer()
    {
        isStarted = false;
        panelTimer.SetActive(false);
    }

    private void Update()
    {
        if (isTimerToStart)
            TimerToStartText();
        if (isStarted)
            TimerToBoomText();
    }

    private void TimerToStartText()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            textTimer.text = "До начала раунда: " + (int)(time * 10) / 10f;
        }
        else
        {
            ownerRoundSystem = FindObjectOfType<OwnerRoundSystem>();
            if (ownerRoundSystem != null)
            {
                ownerRoundSystem.SetRandomPlayerHot();
            }
            isStarted = true;
            time = timeBtwBoomValue;
            isTimerToStart = false;
        }
    }

    private void TimerToBoomText()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            textTimer.text = "До взрыва: " + (int)(time * 10) / 10f;
        }
        else
        {
            ownerRoundSystem = FindObjectOfType<OwnerRoundSystem>();
            if (ownerRoundSystem != null)
            {
                ownerRoundSystem.TimeToBoomPlayer();
            }
            time = timeBtwBoomValue;
        }
    }
}
