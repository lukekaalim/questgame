using System;
using System.Collections.Generic;
using UnityEngine;

namespace Route.Linear
{
	[RequireComponent(typeof(LinearPoint))]
	public class LinearCollider : RouteCollider<LinearRoute>
	{
		public event Action<LinearTraveller, Vector2> OnDetailedEnter;
		public event Action<LinearTraveller, Vector2> OnDetailedCollide;

		[SerializeField]
		Bounds _collisionBounds;

		[SerializeField]
		protected LinearPoint _position;

		public Bounds ColliderBounds
		{
			get
			{
				return _collisionBounds;
			}
		}

		public override RoutePoint<LinearRoute> Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value as LinearPoint;
			}
		}

		public override void Assign(RoutePoint<LinearRoute> newPosition)
		{
			_position = newPosition as LinearPoint;
			ParentRoute.QueueColliderForAddition(this);
		}

		protected override void PointMoved()
		{
			_collisionBounds = new Bounds(_position.Position, _collisionBounds.size);
		}

		public void Assign(LinearRoute newRoute, Vector2 position)
		{
			LinearPoint point = gameObject.AddComponent<LinearPoint>();
			point.Assign(newRoute, position);

			_collisionBounds.center = position;
			Assign(point);
		}

		public override bool TestCollider(RouteCollider<LinearRoute> collider)
		{
			throw new NotImplementedException();
		}

		public override bool TestPoint(RoutePoint<LinearRoute> point)
		{
			throw new NotImplementedException();
		}

		public bool TestTraveller(LinearTraveller traveller, Ray travellerMovement, float distance)
		{
			float collisionDistance;
			if (_collisionBounds.IntersectRay(travellerMovement, out collisionDistance) && collisionDistance <= distance)
			{
				Vector2 collisionPoint = travellerMovement.origin + (travellerMovement.direction * collisionDistance);

				if (_currentlyCollidingTravellers.Contains(traveller))
				{
					TriggerOnCollide(traveller);
					if (OnDetailedEnter != null)
					{
						OnDetailedEnter.Invoke(traveller, collisionPoint);
					}
				}
				else
				{
					TriggerOnEnter(traveller);
					if (OnDetailedEnter != null)
					{
						OnDetailedEnter.Invoke(traveller, collisionPoint);
					}

					TriggerOnCollide(traveller);
					if (OnDetailedCollide != null)
					{
						OnDetailedCollide.Invoke(traveller, collisionPoint);
					}

					_currentlyCollidingTravellers.Add(traveller);
				}

				return true;
			}
			else
			{
				if (_currentlyCollidingTravellers.Contains(traveller))
				{
					_currentlyCollidingTravellers.Remove(traveller);
					TriggerOnExit(traveller);
				}
			}
			return false;
		}

		protected void OnDrawGizmos()
		{

			 /*
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

			//Gizmos.color = Color.red;

			/*
			Gizmos.DrawCube(
				_position.GetWorldSpacePosition(-_collisionBounds.extents.x),
				new Vector3(0.1f, _collisionBounds.size.y, 0.1f));

			Gizmos.DrawCube(
				_position.GetWorldSpacePosition(_collisionBounds.extents.x),
				new Vector3(0.1f, _collisionBounds.size.y, 0.1f));
				*/

			List<Vector3> samples = ParentRoute.LinearTraversable.GetPointSample(
				_collisionBounds.center.x - _collisionBounds.extents.x,
				_collisionBounds.extents.x + _collisionBounds.center.x);

			Matrix4x4 originalMatrix = Gizmos.matrix;
			Gizmos.color = new Color(0.8f, 0.6f, 0, 0.5f);

			for (int i = 0; i < samples.Count - 1; i++)
			{

				Vector3 P0 = samples[i] + new Vector3(0,_collisionBounds.center.y);
				Vector3 P1 = samples[i + 1] + new Vector3(0, _collisionBounds.center.y);
				Vector3 center = Vector3.Lerp(P0, P1, 0.5f);
				Vector3 velocity = P1 - P0;

				if (velocity != Vector3.zero)
				{
					float width = Vector3.Distance(P0 , P1);
					Gizmos.matrix *= Matrix4x4.TRS(center, Quaternion.LookRotation(velocity, Vector3.up), new Vector3(0.1f, _collisionBounds.size.y, width));

					Gizmos.DrawCube(Vector3.zero, Vector3.one);

					Gizmos.matrix = originalMatrix;
				}
			}


			//And draw the regular point
			//base.OnDrawGizmos();
			//Revert back to the original matrix
			Gizmos.matrix = originalMatrix;
		}

	}
}
