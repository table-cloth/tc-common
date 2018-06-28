using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TC
{
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
        public bool IsTimerCountDown { get; private set; }

        // Whether timer is active or not.
        public bool IsRunning { get; private set; }

        // Whether to ceil up millisec, when converting time to seconds.
        // If this is true, 5.1 seconds in float will be converted to 6 seconds in int.
        public bool IsCeilMillisec { get; private set; }

        // Callbacks
        private Action<int> onUpdateIntSecond = null;
        private Action<float> onUpdateFloatSecond = null;
        private Action onComplete = null;

        /// <summary>
        /// Creates the new timer instance and attach to _attachParent.
        /// If _attachParent is null, it will create new GameObject and attach self to it.
        /// </summary>
        /// <returns>The new timer.</returns>
        /// <param name="_attachParent">Attach parent.</param>
        public static Timer CreateNewTimer(GameObject _attachParent)
        {
            Timer timer;

            // Just attach new instance to given object.
            if(_attachParent != null)
            {
                timer = _attachParent.AddComponent<Timer>();
            }
            else
            {
                // Create new game object and attach.
                GameObject gameObject = new GameObject();
                timer = gameObject.AddComponent<Timer>();
                // Update name of object, if it is newly created for this class.
                gameObject.name = timer.GetType().FullName;
            }

            // Set default setting.
            timer.IsTimerCountDown = true;
            timer.IsRunning = false;
            timer.IsCeilMillisec = true;

            return timer;
        }

        /// <summary>
        /// Initialize timer instance.
        /// </summary>
        /// <param name="_startSec">Start sec.</param>
        /// <param name="_endSec">End sec.</param>
        /// <param name="_onUpdateSecondCallback">On update second callback.</param>
        /// <param name="_onUpdateMillisCallback">On update millis callback.</param>
        /// <param name="_onCompleteCallback">On complete callback.</param>
        public void Initialize(
            float _startSec,
            float _endSec,
            Action<int> _onUpdateSecondCallback,
            Action<float> _onUpdateMillisCallback,
            Action _onCompleteCallback)
        {
            SetTimer(_startSec, _endSec);
            SetCallbacks(_onUpdateSecondCallback, _onUpdateMillisCallback, _onCompleteCallback);
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
            IsTimerCountDown = _startSec > _endSec;
            startSec = _startSec;
            endSec = _endSec;
        }

        /// <summary>
        /// Sets the callbacks.
        /// </summary>
        /// <param name="_onUpdateIntSecondCallback">On update int second callback.</param>
        /// <param name="_onUpdateFloatSecondCallback">On update float second callback.</param>
        public void SetCallbacks(
            Action<int> _onUpdateIntSecondCallback,
            Action<float> _onUpdateFloatSecondCallback,
            Action _onComplete)
        {
            onUpdateIntSecond = _onUpdateIntSecondCallback;
            onUpdateFloatSecond = _onUpdateFloatSecondCallback;
            onComplete = _onComplete;
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
            InvokeIntSecondCallback();
            InvokeFloatSecondCallback();
        }

        /// <summary>
        /// Sets whether to ceil up millisec, when converting time to seconds.
        /// If this is true, 5.1 seconds in float will be converted to 6 seconds in int.
        /// </summary>
        /// <param name="_isCeilMillisec">If set to <c>true</c> is ceil millisec.</param>
        public void SetCeilMillisec(bool _isCeilMillisec)
        {
            IsCeilMillisec = _isCeilMillisec;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <param name="_resetCurrentTime">If set to <c>true</c> reset current time.</param>
        public void StartTimer(bool _resetCurrentTime = false)
        {
            IsRunning = true;
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
            IsRunning = false;
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
		/// Gets the start second as int.
		/// </summary>
		/// <returns>The start int sec.</returns>
		public int GetStartIntSec()
		{
			return Convert2IntSec(startSec);
		}

		/// <summary>
		/// Gets the start second as int.
		/// </summary>
		/// <returns>The start int sec.</returns>
		public int GetEndIntSec()
		{
			return Convert2IntSec(endSec);
		}

        /// <summary>
        /// Converts float second into int second format.
        /// </summary>
        /// <returns>The int sec.</returns>
        /// <param name="_sec">Sec.</param>
        private int Convert2IntSec(float _sec)
        {
            return IsCeilMillisec ? Mathf.CeilToInt(_sec) : Mathf.FloorToInt(_sec);
        }

        /// <summary>
        /// Invokes the second callback.
        /// </summary>
        private void InvokeIntSecondCallback()
        {
            if(onUpdateIntSecond != null)
            {
                onUpdateIntSecond(GetCurrentIntSec());
            }
        }

        /// <summary>
        /// Invokes the millisec callback.
        /// </summary>
        private void InvokeFloatSecondCallback()
        {
            if(onUpdateFloatSecond != null)
            {
                onUpdateFloatSecond(GetCurrentFloatSec());
            }
        }

        /// <summary>
        /// Invokes the on complete callback.
        /// </summary>
        private void InvokeOnComplete()
        {
            if(onComplete != null)
            {
                onComplete();
            }
        }

        /// <summary>
        /// Whether time is over.
        /// </summary>
        /// <returns><c>true</c> if this instance is time over; otherwise, <c>false</c>.</returns>
        private bool IsTimeOver()
        {
            return IsTimerCountDown ? currentSec <= endSec : currentSec >= endSec;
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        private void Update()
        {
            // Do nothing if timer is not running.
            if(!IsRunning)
            {
                return;
            }

            // Update actual time.
            int prevSec = GetCurrentIntSec();
            currentSec += IsTimerCountDown ? -Time.deltaTime : Time.deltaTime;
            int newSec = GetCurrentIntSec();

            if(IsTimeOver())
            {
                IsRunning = false;
                currentSec = endSec;
                InvokeOnComplete();
            }

            // Invoke each callbacks.
            if(prevSec != newSec)
            {
                InvokeIntSecondCallback();
            }
            InvokeFloatSecondCallback();
        }
    }
}
