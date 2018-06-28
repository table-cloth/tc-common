using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Simple timer class, capable of sec updates and millisec udpates.
/// </summary>
public class Timer : MonoBehaviour
{
    // Time data.
    private float startSec = 0.0f;
    private float endSec = 0.0f;
    private float currentSec = 0.0f;

    // Whether timer is count down or count up.
    private bool isTimerCountDown = false;

    // Whether timer is active or not.
    private bool isRunning = false;

    // Whether to ceil up millisec, when converting time to seconds.
    // If this is true, 5.1 seconds in float will be converted to 6 seconds in int.
    private bool isCeilMillisec = true;

    // Callbacks
    private Action<int> onUpdateSecond = null;
    private Action<float> onUpdateMillisec = null;

    /// <summary>
    /// Creates the new timer instance and attach to _attachParent.
    /// If _attachParent is null, it will create new GameObject and attach self to it.
    /// </summary>
    /// <returns>The new timer.</returns>
    /// <param name="_attachParent">Attach parent.</param>
    public static Timer CreateNewTimer(GameObject _attachParent)
    {
        // Just attach new instance to given object.
        if(_attachParent != null)
        {
            return _attachParent.AddComponent<Timer>();
        }

        // Create new game object and attach.
        GameObject gameObject = new GameObject();
        Timer timer = gameObject.AddComponent<Timer>();
        // Update name of object, if it is newly created for this class.
        gameObject.name = timer.GetType().FullName;
    }

    /// <summary>
    /// Initialize timer.
    /// </summary>
    /// <param name="_startSec">Start sec.</param>
    /// <param name="_endSec">End sec.</param>
    /// <param name="_onUpdateSecondCallback">On update second callback.</param>
    /// <param name="_onUpdateMillisCallback">On update millis callback.</param>
    public void Initialize(
        float _startSec,
        float _endSec,
        Action<int> _onUpdateSecondCallback,
        Action<float> _onUpdateMillisCallback)
    {
        SetTimer(_startSec, _endSec);
        SetCallbacks(_onUpdateSecondCallback, onUpdateMillisec);
    }

    /// <summary>
    /// Sets the timer count down.
    /// </summary>
    /// <param name="_startSec">Start sec.</param>
    public void SetTimerCountDown(float _startSec)
    {
        SetTimer(_startSec, 0.0f);
    }

    /// <summary>
    /// Sets the timer count up.
    /// </summary>
    /// <param name="_endSec">End sec.</param>
    public void SetTimerCountUp(float _endSec)
    {
        SetTimer(0.0f, _endSec);
    }

    /// <summary>
    /// Sets the timer.
    /// </summary>
    /// <param name="_startSec">Start sec.</param>
    /// <param name="_endSec">End sec.</param>
    public void SetTimer(float _startSec, float _endSec)
    {
        isTimerCountDown = _startSec > _endSec;
        startSec = _startSec;
        endSec = _endSec;
    }


    /// <summary>
    /// Sets the callbacks.
    /// </summary>
    /// <param name="_onUpdateSecondCallback">On update second callback.</param>
    /// <param name="_onUpdateMillisCallback">On update millis callback.</param>
    public void SetCallbacks(Action<int> _onUpdateSecondCallback, Action<float> _onUpdateMillisCallback)
    {
        onUpdateSecond = _onUpdateSecondCallback;
        onUpdateMillisec = _onUpdateMillisCallback;
    }

    /// <summary>
    /// Resets the current time to start second.
    /// </summary>
    public void ResetCurrentTime()
    {
        SetCurrentTime(startSec);
    }

    /// <summary>
    /// Sets the current time to specified second.
    /// </summary>
    /// <param name="_sec">Sec.</param>
    public void SetCurrentTime(float _sec)
    {
        currentSec = _sec;
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    /// <param name="_resetCurrentTime">If set to <c>true</c> reset current time.</param>
    public void StartTimer(bool _resetCurrentTime = false)
    {
        isRunning = true;
        if(_resetCurrentTime)
        {
            ResetCurrentTime();
        }
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    /// <param name="_resetCurrentTime">If set to <c>true</c> reset current time.</param>
    public void StopTimer(bool _resetCurrentTime = false)
    {
        isRunning = false;
        if(_resetCurrentTime)
        {
            ResetCurrentTime();
        }
    }

    /// <summary>
    /// Gets the current second as float.
    /// </summary>
    /// <returns>The current float sec.</returns>
    public float GetCurrentFloatSec()
    {
        return currentSec;
    }

    /// <summary>
    /// Gets the current second as int.
    /// </summary>
    /// <returns>The current int sec.</returns>
    public int GetCurrentIntSec()
    {
        return Convert2IntSec(currentSec);
    }

    /// <summary>
    /// Converts float second into int second format.
    /// </summary>
    /// <returns>The int sec.</returns>
    /// <param name="_sec">Sec.</param>
    private int Convert2IntSec(float _sec)
    {
        return isCeilMillisec ? Mathf.CeilToInt(_sec) : Mathf.FloorToInt(_sec);
    }

    /// <summary>
    /// Invokes all callbacks set.
    /// </summary>
    private void InvokeCallbacks()
    {
        InvokeSecondCallback();
    }

    /// <summary>
    /// Invokes the second callback.
    /// </summary>
    private void InvokeSecondCallback()
    {
        if(onUpdateSecond != null)
        {
            onUpdateSecond(GetCurrentIntSec());
        }
    }

    /// <summary>
    /// Invokes the millisec callback.
    /// </summary>
    private void InvokeMillisecCallback()
    {
        if(onUpdateMillisec != null)
        {
            onUpdateMillisec(GetCurrentFloatSec());
        }
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        // Do nothing if timer is not running.
        if(!isRunning)
        {
            return;
        }

        // Update actual time.
        int prevSec = GetCurrentIntSec();
        currentSec += isTimerCountDown ? -Time.deltaTime : Time.deltaTime;
        int newSec = GetCurrentIntSec();

        // Invoke each callbacks.
        if(prevSec != newSec)
        {
            InvokeSecondCallback();
        }
        InvokeMillisecCallback();
    }
}
