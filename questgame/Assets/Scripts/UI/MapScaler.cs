using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapScaler : MonoBehaviour {

	[SerializeField]
	Vector3 naturalMapScale;

	[SerializeField]
	Rect naturalScalerScale;

	[SerializeField]
	Transform mapTransform;

	[SerializeField]
	bool wilRecalculateOnStart = true;

	[SerializeField, HideInInspector]
	RectTransform scalerTransform;

	void Awake()
	{
		if (wilRecalculateOnStart)
		{
			scalerTransform = GetComponent<RectTransform>();
			mapTransform = transform.GetChild(0).transform;

			naturalMapScale = mapTransform.localScale;
			naturalScalerScale = scalerTransform.rect;
		}
	}

	void Start()
	{
		if (wilRecalculateOnStart)
		{
			naturalMapScale = mapTransform.localScale;
			naturalScalerScale = scalerTransform.rect;
		}
	}

	void Update () {

		float xScale = scalerTransform.rect.width / naturalScalerScale.width * naturalMapScale.x;
		float yScale = scalerTransform.rect.height / naturalScalerScale.height * naturalMapScale.y;

		//Debug.Log(scalerTransform.rect.height / naturalScalerScale.height);

		mapTransform.localScale = new Vector3(xScale, yScale, mapTransform.localScale.z);
	}
}
