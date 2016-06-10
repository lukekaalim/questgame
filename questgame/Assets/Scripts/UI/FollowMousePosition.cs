using UnityEngine;
using UnityEngine.UI;

class FollowMousePosition : MonoBehaviour
{
	RectTransform rect;

	RectTransform parentCanvas;

	void Awake()
	{
		parentCanvas = transform.parent.GetComponent<RectTransform>();
		rect = GetComponent<RectTransform>();
	}

	void Update()
	{
		Vector2 newPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas, Input.mousePosition, Camera.main, out newPosition);
		rect.anchoredPosition = newPosition;
	}
}
