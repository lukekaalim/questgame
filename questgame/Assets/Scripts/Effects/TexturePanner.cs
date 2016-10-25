using UnityEngine;

public class TexturePanner : MonoBehaviour {

	[SerializeField]
	Material materialToPan;

	[SerializeField]
    Vector2 panAmount;

	void Update ()
	{
        materialToPan.SetTextureOffset("_MainTex",panAmount * Time.realtimeSinceStartup);
	}
}
