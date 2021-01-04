using System;
using UnityEngine;

public class UiTransitionController : MonoBehaviour
{
	[SerializeField] private float transitionTimeInSeconds;
	[SerializeField] private GameObject transitionPanel;
	private bool _isTransitioning;
	private float _realTimeWhenTimerStarted;
	private float _targetTime;
	private Action _endTransitionCallback;

	void Update()
	{
		if (_isTransitioning)
		{
			if (Time.realtimeSinceStartup > _targetTime)
			{
				_isTransitioning = false;
				_endTransitionCallback?.Invoke();
				transitionPanel.SetActive(false);
			}
		}
	}

	public void BeginTransition(Action beginTransitionCallback, Action endTransitionCallback)
	{
		_targetTime = Time.realtimeSinceStartup + transitionTimeInSeconds;
		_isTransitioning = true;
		_endTransitionCallback = endTransitionCallback;
		transitionPanel.SetActive(true);

		beginTransitionCallback?.Invoke();
	}
}
