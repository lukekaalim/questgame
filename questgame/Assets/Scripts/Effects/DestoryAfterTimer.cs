using UnityEngine;
using System.Collections;

public class DestoryAfterTimer : MonoBehaviour {

	[SerializeField]
	float timeToLive;

	float timeSpentLiving = 0;

	void Update () {

		timeSpentLiving += Time.deltaTime;
		if (timeSpentLiving > timeToLive)
		{
			Destroy(gameObject);
		}

	}
}
