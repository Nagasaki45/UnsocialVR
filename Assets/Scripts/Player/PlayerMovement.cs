using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

	private Transform camera;


	private void Start()
	{
		camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
	}


	private void Update ()
	{
		if (isLocalPlayer)
			LocalPlayerUpdate ();
		else
			RemotePlayerUpdate ();
	}


	private void LocalPlayerUpdate()
	{
		transform.position = camera.position;
		transform.rotation = camera.rotation;
	}


	private void RemotePlayerUpdate()
	{
	}
}
