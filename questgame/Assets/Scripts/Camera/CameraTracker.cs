using UnityEngine;
using System.Collections;

namespace Cameras
{
	[ExecuteInEditMode]
	public class CameraTracker : MonoBehaviour
	{
		[SerializeField]
		Transform _target;

		public Transform Target
		{
			get
			{
				return _target;
			}
			set
			{
				_target = value;
			}
		}

		void Update()
		{
			if (_target != null)
			{
				transform.LookAt(_target, Vector3.up);
			}
		}
	}
}