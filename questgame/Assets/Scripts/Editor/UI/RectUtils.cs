using UnityEngine;

namespace Utils
{
	public static class RectUtils
	{
		public static Rect LeftHalf(Rect parentRect)
		{
			return new Rect(parentRect.x, parentRect.y, parentRect.width / 2, parentRect.height);
		}

		public static Rect RightHalf(Rect parentRect)
		{
			return new Rect(parentRect.x + (parentRect.width / 2), parentRect.y, parentRect.width / 2, parentRect.height);
		}

		public static Rect TopHalf(Rect parentRect)
		{
			return new Rect(parentRect.x, parentRect.y, parentRect.width, parentRect.height/2);
		}

		public static Rect BottomHalf(Rect parentRect)
		{
			return new Rect(parentRect.x, parentRect.y + (parentRect.height / 2), parentRect.width, parentRect.height / 2);
		}

	}
}
