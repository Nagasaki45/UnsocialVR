using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


[System.Serializable]
public class PlayerData {

	public Vector3 chestPosition;
	public Quaternion chestRotation;
	public Vector3 headPosition;
	public Quaternion headRotation;
	public Vector3 leftHandPosition;
	public Quaternion leftHandRotation;
	public Vector3 rightHandPosition;
	public Quaternion rightHandRotation;
	public int attention;
	public bool isTalking;
	public string state;


	public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}


	public static Dictionary<uint, PlayerData> FromDictJson(string json)
	{
		Dictionary<uint, PlayerData> d = new Dictionary<uint, PlayerData> ();
		JSONNode root = JSON.Parse (json);
		IEnumerator enumerator = root.AsArray.GetEnumerator ();
		while (enumerator.MoveNext ())
		{
			JSONNode current = (JSONNode)enumerator.Current;
			PlayerData pd = new PlayerData ();
			pd.chestPosition = new Vector3 (current ["chestPosition"] ["x"].AsFloat, current ["chestPosition"] ["y"].AsFloat, current ["chestPosition"] ["z"].AsFloat);
			pd.chestRotation = new Quaternion (current ["chestRotation"] ["x"].AsFloat, current ["chestRotation"] ["y"].AsFloat, current ["chestRotation"] ["z"].AsFloat, current ["chestRotation"] ["w"].AsFloat);
			pd.headPosition = new Vector3 (current ["headPosition"] ["x"].AsFloat, current ["headPosition"] ["y"].AsFloat, current ["headPosition"] ["z"].AsFloat);
			pd.headRotation = new Quaternion (current ["headRotation"] ["x"].AsFloat, current ["headRotation"] ["y"].AsFloat, current ["headRotation"] ["z"].AsFloat, current ["headRotation"] ["w"].AsFloat);
			pd.leftHandPosition = new Vector3 (current ["leftHandPosition"] ["x"].AsFloat, current ["leftHandPosition"] ["y"].AsFloat, current ["leftHandPosition"] ["z"].AsFloat);
			pd.leftHandRotation = new Quaternion (current ["leftHandRotation"] ["x"].AsFloat, current ["leftHandRotation"] ["y"].AsFloat, current ["leftHandRotation"] ["z"].AsFloat, current ["leftHandRotation"] ["w"].AsFloat);
			pd.rightHandPosition = new Vector3 (current ["rightHandPosition"] ["x"].AsFloat, current ["rightHandPosition"] ["y"].AsFloat, current ["rightHandPosition"] ["z"].AsFloat);
			pd.rightHandRotation = new Quaternion (current ["rightHandRotation"] ["x"].AsFloat, current ["rightHandRotation"] ["y"].AsFloat, current ["rightHandRotation"] ["z"].AsFloat, current ["rightHandRotation"] ["w"].AsFloat);
			pd.attention = current ["attention"].AsInt;
			pd.isTalking = current ["isTalking"].AsBool;
			pd.state = current ["state"];
			d.Add ((uint) current ["id"].AsInt, pd);
		}
		return d;
	}
}
