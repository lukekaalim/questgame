using UnityEngine;

namespace Serialization
{
	//I hate unity for making me do this
	[System.Serializable]
	public class SerializableDictionaryStringInt : SerializableDictionary<string, int> { }
}
