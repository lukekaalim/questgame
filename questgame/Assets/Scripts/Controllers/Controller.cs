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
	public enum Intention
	{
		None = 0,
		MoveLeft = 1,
		MoveRight = 2,
		MoveUp = 3,
		MoveDown = 4,
		Tap = 5,
		Shake = 6,
		Hold = 7
	}

	public class Controller : MonoBehaviour
	{
		[SerializeField]
		Traveller _controlledTraveller;

		[SerializeField]
		List<float> _intentionWeights;

		[SerializeField]
		Character _character;

		[SerializeField]
		float _speed;

		public ReadOnlyCollection<float> IntentionWeights
		{
			get
			{
				return _intentionWeights.AsReadOnly();
			}
		}

		public float GetIntentionWeight(Intention intent)
		{
			return _intentionWeights[(int)intent];
		}

		public float Speed
		{
			get
			{
				return _speed;
			}
		}

		public void ConsumeIntention(Intention intentionToConsume)
		{
			_intentionWeights[(int)intentionToConsume] = 0;
		}

		void Awake()
		{
			_controlledTraveller.SetController(this);
			_intentionWeights = new List<float>(new float[Enum.GetNames(typeof(Intention)).Length]);
		}

		void Update()
		{
			_controlledTraveller = _controlledTraveller.UpdateTraveller();

			if(Input.touchCount > 0)
			{
				_intentionWeights[(int)Intention.Tap] = 3;
			}
		}
	}
}