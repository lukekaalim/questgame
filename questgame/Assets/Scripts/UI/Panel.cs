using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

[RequireComponent(typeof(Animator))]
public class Panel : MonoBehaviour
{
	[SerializeField]
	protected List<string> _states = new List<string>();

	public bool refreshPanelOnLoad = false;

	public ReadOnlyCollection<string> States
	{
		get
		{
			return _states.AsReadOnly();
		}
	}

	[SerializeField]
	protected int _currentState;
	public int CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			SetState(value);
		}
	}

	[SerializeField]
	public event Action<Panel, int> OnSwitch;

	Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	void Start()
	{
		if (refreshPanelOnLoad)
		{
			SetState(_currentState);
		}
	}

	public virtual void SetState(int newState)
	{
		if (newState >= _states.Count || newState < 0)
		{
			return;
		}

		if (OnSwitch != null)
		{
			OnSwitch.Invoke(this, newState);
		}

		_currentState = newState;
		animator.SetTrigger(_states[newState]);
	}

	public void ToggleBinaryStates()
	{
		_currentState = _currentState == 0 ? 1 : 0;

		animator.SetTrigger(_states[_currentState]);
	}

	public void NextState()
	{
		_currentState = _currentState + 1 < _states.Count ? _currentState + 1 : 0;

		animator.SetTrigger(_states[_currentState]);
	}

#if UNITY_EDITOR
	public void AddState(string newStateName)
	{
		_states.Add(newStateName);
	}

	public void RemoveState(int stateIndex)
	{
		_states.RemoveAt(stateIndex);
	}

	public void RenameState(int stateIndex, string newName)
	{
		_states[stateIndex] = newName;
	}

	public void SetDefaultState(int newDefaultState)
	{
		_currentState = newDefaultState < States.Count && newDefaultState >= 0 ? newDefaultState : _currentState;
	}
#endif
}
