using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TC;

/// <summary>
/// Sample timer manager.
/// </summary>
public class SampleTimerManager : MonoBehaviour
{
    [SerializeField]
    private float maxSec = 10.0f;
    [SerializeField]
    private float minSec = 0.0f;

    [SerializeField]
    private Text timerSecText, timerMillisecText, timerCompleteText;

    // Timer instance.
    private Timer timer = null;

    /// <summary>
    /// Resets the timer.
    /// </summary>
    public void ResetTimer()
    {
        timer.ResetCurrentTime();
        timerCompleteText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        timer.StartTimer();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void StopTimer()
    {
        timer.StopTimer();
    }

    /// <summary>
    /// Switchs the count up down.
    /// </summary>
    public void SwitchCountUpDown()
    {
        if(timer.IsTimerCountDown)
        {
            timer.SetTimerCountUp(maxSec);
        }
        else
        {
            timer.SetTimerCountDown(maxSec);
        }
    }

    /// <summary>
    /// Switchs ceil floor setting.
    /// </summary>
    public void SwitchCeilFloor()
    {
        timer.SetCeilMillisec(!timer.IsCeilMillisec);
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        timer = Timer.CreateNewTimer(gameObject);
        timer.Initialize(minSec, maxSec, (int _sec) =>
        {
            timerSecText.text = _sec.ToString();
        }, (float _sec) =>
        {
            timerMillisecText.text = _sec.ToString("N");
        }, () =>
        {
            timerCompleteText.gameObject.SetActive(true);
        });
        StartTimer();
    }
}
