using TMPro;
using UnityEngine;

public class PlayerTimerText: MonoBehaviour
{
    public TextMeshProUGUI textTimer;
    public bool isTimerToStart;
    public bool isStarted;
    private float startTimeValue = 10f;
    private float timeBtwBoomValue = 15f;
    private float time;

    private OwnerRoundSystem ownerRoundSystem;

    private void Start()
    {
        ownerRoundSystem = FindObjectOfType<OwnerRoundSystem>();
    }

    public void StartTimer()
    {
        isTimerToStart = true;
        time = startTimeValue;
    }

    public void StopTimer()
    {
        isStarted = false;
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
            if (ownerRoundSystem != null)
            {
                ownerRoundSystem.TimeToBoomPlayer();
            }
            time = timeBtwBoomValue;
        }
    }
}
