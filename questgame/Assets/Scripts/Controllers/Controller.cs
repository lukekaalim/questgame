using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Route;
using Characters;
using Level;
using Serialization;

namespace Controllers
{
	[Flags]
	public enum Intention
	{
		None = 0,
		MoveLeft = 1 << 0,
		MoveRight = 1 << 1,
		MoveUp = 1 << 2,
		MoveDown = 1 << 3,
		Tap = 1 << 4,
		Shake = 1 << 5,
		Hold = 1 << 6,
	}

	public class Controller : MonoBehaviour
	{
		[SerializeField]
		SIControllable _controlled;

		[SerializeField]
		Character _character;

		[SerializeField]
		float _speed;

		int touchesLastFrame = 0;
		Intention _intentForThisFrame = Intention.None;

		public float Speed
		{
			get
			{
				return _speed;
			}
		}

		void OnEnable()
		{
			if (_controlled.Value != null)
			{
				_controlled.Value.SetController(this);
			}
		}

		void OnDisable()
		{
			if (_controlled.Value != null)
			{
				_controlled.Value.SetController(null);
			}
		}

		void Update()
		{
			if (_controlled != null)
			{
				ProcessInput();
			}
		}

		protected virtual void ProcessInput()
		{
			_intentForThisFrame = Intention.None;

			if (touchesLastFrame < Input.touchCount)
			{
				_intentForThisFrame = _intentForThisFrame | Intention.Tap;
			}

			_controlled.Value = _controlled.Value.ApplyInput(_intentForThisFrame);

			touchesLastFrame = Input.touchCount;
		}

		public void ConsumeIntention(Intention intentionToConsume)
		{
			_intentForThisFrame = _intentForThisFrame & ~intentionToConsume;
		}
	}
}