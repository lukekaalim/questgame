using System;
using UnityEngine;

namespace DisplayBinding
{
	public abstract class DisplayBase : MonoBehaviour
	{
		public abstract void Refresh();
	}

	public abstract class DisplayBase<T> : DisplayBase where T : IEquatable<T>
	{
		[SerializeField]
		protected T _currentValue;
		public virtual T CurrentValue
		{
			get
			{
				return _currentValue;
			}

			set
			{
				if (value == null || !value.Equals(_currentValue))
				{
					OnValueChange(_currentValue, value);

					_currentValue = value;
				}
			}
		}

		public override void Refresh()
		{
			CurrentValue = _currentValue;
			OnValueChange(_currentValue, _currentValue);
		}

		virtual protected void OnValueChange(T oldValue, T newValue) { }
	}
}