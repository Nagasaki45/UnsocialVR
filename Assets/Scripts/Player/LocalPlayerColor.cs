using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalPlayerColor : NetworkBehaviour {

	public Color color;


	public override void OnStartLocalPlayer()
	{
		GetComponentInChildren<MeshRenderer> ().material.color = color;
	}
}
