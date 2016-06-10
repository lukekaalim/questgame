using System;
using UnityEngine;

namespace DisplayBinding
{
	public class LimitedDisplay<T> : DisplayBase<T> where T : IEquatable<T>, IComparable<T>
	{
		[SerializeField]
		protected T _min;
		[SerializeField]
		protected T _max;

		public override T CurrentValue
		{
			get
			{
				return _currentValue;
			}

			set
			{
				value = ClampValue(value);
				if (!value.Equals(_currentValue))
				{
					OnValueChange(_currentValue, value);
					_currentValue = value;
				}
			}
		}
		
		public void SetLimits(T min, T max)
		{
			_min = min;
			_max = max;
			_currentValue = ClampValue(_currentValue);
		}

		protected T ClampValue(T value)
		{
			if(value.CompareTo(_min) < 0)
			{
				return _min;
			}

			if (value.CompareTo(_max) > 0)
			{
				return _max;
			}

			return value;
		}
	}
}