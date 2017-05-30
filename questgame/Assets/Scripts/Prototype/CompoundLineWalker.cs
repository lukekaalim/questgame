using UnityEngine;

using Shapes;

public class CompoundLineWalker : MonoBehaviour
{
	[SerializeField]
	CompoundLine line;

	[SerializeField]
	CompoundLine.Position position = new CompoundLine.Position(0, 0);

	[SerializeField]
	float speed = 1;

	private void Update()
	{
		position += speed * Time.deltaTime;

		position = line.UpdatePosition(position);

		transform.position = line.ResolvePosition(position);
	}
}
