using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableTransform {

	public Vector3 position;
	public Quaternion rotation;

	public static string ToJson(Transform transform)
	{
		SerializableTransform st = new SerializableTransform();
		st.position = transform.position;
		st.rotation = transform.rotation;
		return JsonUtility.ToJson(st);
	}

	public static SerializableTransform FromJson(string json)
	{
		return JsonUtility.FromJson<SerializableTransform> (json);
	}
}
