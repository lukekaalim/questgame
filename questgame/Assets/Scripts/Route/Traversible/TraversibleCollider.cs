using System;
using UnityEngine;

namespace Route.Traversible
{
	public class TraversibleCollider : RouteCollider<TraversibleRoute>
	{
		[SerializeField]
		Bounds _collisionBounds;

		public override void Assign(TraversibleRoute newRoute, RoutePoint<TraversibleRoute> newPosition)
		{
			_parentRoute = newRoute;
			_position = newPosition;
			_parentRoute.QueueColliderForAddition(this);
		}

		public void Assign(TraversibleRoute newRoute, Vector2 position)
		{
			TraversiblePoint point = gameObject.AddComponent<TraversiblePoint>();
			point.Assign(newRoute, position);

			_collisionBounds.center = position;
			Assign(newRoute, point);
		}

		public override bool TestCollider(RouteCollider<TraversibleRoute> collider)
		{
			throw new NotImplementedException();
		}

		public override bool TestPoint(RoutePoint<TraversibleRoute> point)
		{
			throw new NotImplementedException();
		}

		public bool TestTraveller(Traveller traveller, Ray travellerMovement, float distance)
		{
			float collisionDistance;
			if (_collisionBounds.IntersectRay(travellerMovement, out collisionDistance) && collisionDistance <= distance)
			{
				Vector2 collisionPoint = travellerMovement.origin + (travellerMovement.direction * collisionDistance);

				if (_currentlyCollidingTravellers.Contains(traveller))
				{
					TriggerOnCollide(traveller);
				}
				else
				{
					TriggerOnEnter(traveller);
					_currentlyCollidingTravellers.Add(traveller);
				}

				return true;
			}
			return false;
		}
		/*
		protected override void OnDrawGizmos()
		{
			/*
			 *
			//Get the world-space position of the point via a semi-expensive call the route
			Vector3 position = GetDebugPosition();
			//Store the original matrix
			Matrix4x4 originalMatrix = Gizmos.matrix;

			//Get the velocity(direction) of the point on the traversable
			Vector3 velocity = _parent.LinearTraversable.GetVelocityAtIndex(_parent.LinearTraversable.GetIndexAtPoint(CollisionBounds.center.x));


			//Modify the matrix to point it at the right postiion
			Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(velocity, Vector3.up), Vector3.one);

			//Draw Extents Solid
			Gizmos.color = new Color(0.8f, 0.6f, 0, 0.5f);
			Gizmos.DrawCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y * 2, _collisionBounds.extents.x * 2));

			//Draw Extents Outline
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.1f, _collisionBounds.extents.y * 2, _collisionBounds.extents.x * 2));

			//Revert back to the original matrix
			Gizmos.matrix = originalMatrix;

			*/

			//Draw the Extents Edges in proper space, since the previous extents solid won't warp along the contours of the traversible
			/*
			Gizmos.color = Color.red;
			Gizmos.DrawCube(_position.GetWorldSpacePosition(-_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y * 2, 0.1f));
			Gizmos.DrawCube(_position.GetWorldSpacePosition(_collisionBounds.extents.x), new Vector3(0.1f, _collisionBounds.extents.y * 2, 0.1f));

			//And draw the regular point
			base.OnDrawGizmos();

		}
		*/
	}
}
