using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {

	private Transform cameraTransform;


	private void Start()
	{
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
	}


	private void Update ()
	{
		transform.position = cameraTransform.position;
		transform.rotation = cameraTransform.rotation;
	}
}
