using UnityEngine;

namespace Test
{
	class DictionaryTester : MonoBehaviour
	{
		[SerializeField]
		InspectorTester tester;

		[SerializeField]
		string key;

		void Start()
		{
			Debug.Log(tester.dict[key].ToString());
		}
	}
}
