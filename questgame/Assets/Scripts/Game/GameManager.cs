using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game
{
	public class GameManager : MonoBehaviour
	{
		private static GameManager _instance;
		public static GameManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("Game Manager", typeof(GameManager)).GetComponent<GameManager>();
				}
				return _instance;
			}
		}

		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public GameManager()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
			}
		}
	}
}