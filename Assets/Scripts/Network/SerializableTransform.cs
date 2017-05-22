﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


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

	public static Dictionary<uint, SerializableTransform> FromDictJson(string json)
	{
		Dictionary<uint, SerializableTransform> d = new Dictionary<uint, SerializableTransform> ();
		JSONNode root = JSON.Parse (json);
		IEnumerator enumerator = root.AsArray.GetEnumerator ();
		while (enumerator.MoveNext ())
		{
			JSONNode current = (JSONNode)enumerator.Current;
			SerializableTransform st = new SerializableTransform ();
			st.position = new Vector3 (current ["position"] ["x"].AsFloat, current ["position"] ["y"].AsFloat, current ["position"] ["z"].AsFloat);
			st.rotation = new Quaternion (current ["rotation"] ["x"].AsFloat, current ["rotation"] ["y"].AsFloat, current ["rotation"] ["z"].AsFloat, current ["rotation"] ["w"].AsFloat);
			d.Add ((uint) current ["id"].AsInt, st);
		}
		return d;
	}
}
