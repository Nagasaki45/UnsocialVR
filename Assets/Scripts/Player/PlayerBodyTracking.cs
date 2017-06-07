using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerBodyTracking : NetworkBehaviour {

	public string trackingDeviceTag;
	public Transform gameObjectTransform;
	private Transform trackingDeviceTransform;


	void Start ()
	{
		if (isLocalPlayer && SceneManager.GetActiveScene ().name != "Simulator")
		{
			trackingDeviceTransform = GameObject.FindGameObjectWithTag (trackingDeviceTag).GetComponent<Transform> ();
		}
	}
	

	void Update ()
	{
		if (isLocalPlayer && SceneManager.GetActiveScene ().name != "Simulator")
		{
			gameObjectTransform.position = trackingDeviceTransform.position;
			gameObjectTransform.rotation = trackingDeviceTransform.rotation;
		}
	}
}
