using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerColor : MonoBehaviour {

	public Color color;


	public void Start ()
	{
		GetComponentInChildren<MeshRenderer> ().material.color = color;
	}
}
