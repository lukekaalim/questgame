using UnityEngine;
using Shapes;
using Serialization;

namespace Test
{
	[ExecuteInEditMode]
	class CompoundLineWalker : MonoBehaviour
	{
		[SerializeField]
		CompoundLine path = null;

		[SerializeField]
		float position;

		[SerializeField]
		bool update = true;

		[SerializeField]
		float speed = 0;


		void Start()
		{
			position = 0;
			if (path == null)
			{
				update = false;
			}
		}

		void Update()
		{
			if (update)
			{
				transform.position = path.GetPositionFromDistance(position);

				position += speed * Time.deltaTime;
			}
		}
	}
}
