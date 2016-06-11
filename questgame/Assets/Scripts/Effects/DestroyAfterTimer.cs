using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour {

	[SerializeField]
	float timeToLive = 0;

	float timeSpentLiving = 0;

	void Update ()
	{
		timeSpentLiving += Time.deltaTime;
		if (timeSpentLiving > timeToLive)
		{
			Destroy(gameObject);
		}
	}
}
