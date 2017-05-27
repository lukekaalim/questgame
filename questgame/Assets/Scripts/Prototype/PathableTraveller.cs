using UnityEngine;
using Shapes;
using Serialization;

namespace Test
{
	[ExecuteInEditMode]
	class PathableTraveller : MonoBehaviour
	{
		[SerializeField]
		SIPathable path;

		[SerializeField]
		float position;

		[SerializeField]
		float totalTime;

		[SerializeField]
		float currentTime;

		[SerializeField]
		bool update = true;


		void Start()
		{
			position = 0;
			if (path == null || path.Value == null)
			{
				update = false;
			}
			currentTime = 0;
		}

		void Update()
		{
			if (update)
			{
				currentTime += Time.deltaTime;

				position = Mathf.Lerp(0, 1, Mathf.Clamp(currentTime / totalTime, 0, 1));

				transform.position = path.Value.GetPointOnPath(position, true);
			}
		}
	}
}
