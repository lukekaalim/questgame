using UnityEngine;

using Route;

namespace Level
{
	public class Coin : MonoBehaviour
	{
		[SerializeField]
		RoutePoint _pointInLevel = null;

		void Awake()
		{
			_pointInLevel.OnActivation += OnCollectCoin;
		}

		private void OnCollectCoin(Traveller traveller)
		{
			Debug.Log("Coin Collected!");
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
				TraversibleRoute traversibleRoute = UnityEditor.Selection.activeGameObject.GetComponent<TraversibleRoute>();
				if (traversibleRoute != null)
				{
					TraversibleRoutePoint point = gameObject.AddComponent<TraversibleRoutePoint>();
					point.Assign(traversibleRoute);
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
}
