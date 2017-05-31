using UnityEngine;

using Route;

namespace Level
{
	/*
	public class Coin : MonoBehaviour
	{
		[SerializeField]
		RouteCollider _pointInLevel;

		void OnEnable()
		{
			if (_pointInLevel == null)
			{
				_pointInLevel = GetComponent<RouteCollider>();
			}
			_pointInLevel.OnEnter += OnCollectCoin;
		}

		void OnDisable()
		{
			_pointInLevel.OnEnter -= OnCollectCoin;
		}

		private void OnCollectCoin(Traveller traveller)
		{
			Destroy(gameObject);
		}

#if UNITY_EDITOR

		[UnityEditor.MenuItem("Level/Make Coin")]
		static void CreateCustomGameObject(UnityEditor.MenuCommand menuCommand)
		{
			// Create a custom game object
			GameObject gameObject = new GameObject("Coin");
			Coin newCoin = gameObject.AddComponent<Coin>();

			if (UnityEditor.Selection.activeGameObject != null)
			{
				Route.Linear.LinearRoute traversibleRoute = UnityEditor.Selection.activeGameObject.GetComponent<Route.Linear.LinearRoute>();
				if (traversibleRoute != null)
				{
					Route.Linear.LinearCollider point = gameObject.AddComponent<Route.Linear.LinearCollider>();
					point.Assign(traversibleRoute, new Vector2(0,0));
					newCoin._pointInLevel = point;
				}
			}

			// Ensure it gets reparented if this was a context click (otherwise does nothing)
			UnityEditor.GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			// Register the creation in the undo system
			UnityEditor.Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
		}

#endif
	}
	*/
}
