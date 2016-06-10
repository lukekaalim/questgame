using System;
using UnityEngine;

[ExecuteInEditMode]
class PanelStayWithinBounds : MonoBehaviour
{
	RectTransform rect;

	[SerializeField]
	RectTransform bounds;

	RectTransform childRect;

	void Awake()
	{
		rect = GetComponent<RectTransform>();

		if (bounds == null)
		{
			bounds = transform.parent.GetComponent<RectTransform>();
		}

		childRect = transform.GetChild(0).GetComponent<RectTransform>();

		childRect.anchoredPosition = Vector3.zero;
		childRect.sizeDelta = Vector3.one;
		childRect.anchorMin = Vector3.zero;
		childRect.anchorMax = Vector3.one;
	}

	void Update()
	{
		float localRight = rect.anchoredPosition.x + rect.sizeDelta.x;
		float boundRight = bounds.anchoredPosition.x + bounds.sizeDelta.x;

		float localLeft = rect.anchoredPosition.x;
		float boundLeft = bounds.anchoredPosition.x;

		float localTop = rect.anchoredPosition.y;
		float boundTop = bounds.anchoredPosition.y;

		float localBottom = rect.anchoredPosition.y - rect.sizeDelta.y;
		float boundBottom = bounds.anchoredPosition.y - bounds.sizeDelta.y;


		if (localRight > boundRight)
		{
			childRect.anchoredPosition = new Vector2(-(localRight - boundRight), 0);
		}
		else if (localLeft < boundLeft)
		{
			childRect.anchoredPosition = new Vector2(boundLeft - localLeft, 0);
		}
		else if (localTop > boundTop)
		{
			childRect.anchoredPosition = new Vector2(0,-(localTop - boundTop));
		}
		else if (localBottom < boundBottom)
		{
			childRect.anchoredPosition = new Vector2(0,boundBottom - localBottom);
		}
		else
		{
			childRect.anchoredPosition = Vector3.zero;
		}
	}
}
