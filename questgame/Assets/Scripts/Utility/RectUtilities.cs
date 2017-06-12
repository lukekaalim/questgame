using UnityEngine;

namespace Utility
{
	public static class RectUtilities
	{
		public static Rect AddHorizontalPadding(this Rect rect, float padding)
		{
			rect.xMin += padding;
			rect.xMax -= padding;
			return rect;
		}

		public static Rect AddVerticalPadding(this Rect rect, float padding)
		{
			rect.yMin -= padding;
			rect.yMax += padding;
			return rect;
		}

		public static Rect AddPadding(this Rect rect, float horizontalPadding, float verticalPadding)
		{
			rect.xMin += horizontalPadding;
			rect.xMax -= horizontalPadding;
			rect.yMin -= verticalPadding;
			rect.yMax += verticalPadding;
			return rect;
		}
		public static Rect AddPadding(this Rect rect, float padding)
		{
			rect.xMin += padding;
			rect.xMax -= padding;
			rect.yMin -= padding;
			rect.yMax += padding;
			return rect;
		}

		public static Rect CreatePositionRect(Vector2 center, Vector2 size)
		{
			return new Rect(center.x - (size.x / 2), center.y - (size.y / 2), size.x, size.y); 
		}

		public static Vector2 GetRelativePosition(this Rect rect, Vector2 absolutePosition)
		{
			float horizontal = (absolutePosition.x - rect.x) / rect.width;
			float vertical = (absolutePosition.y - rect.y) / rect.height;

			return new Vector2(horizontal, vertical);
		}
	}
}
