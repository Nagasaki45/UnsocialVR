﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerController : NetworkBehaviour {

	// Keep local player info as static
	// One instance of the class per app instance will work
	public static uint localPlayerNetId;
	public static Dictionary<uint, SerializableTransform> remotePlayersTransforms;

	// Local player settings
	public string serverUrl;
	public float sleepBetweenRequests;

	// Remote players settings
	public Color color;
	public float smoothing;

	// Using keyboard in simulator
	public float speed;
	public float angularSpeed;
	public float defaultHeight;

	private Transform cameraTransform;
	private SerializableTransform targetTransform;


	private void Start()
	{
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		if (isLocalPlayer)
		{
			localPlayerNetId = netId.Value;
			GetComponentInChildren<MeshRenderer> ().material.color = color;
			StartCoroutine(CommunicateForever ());
		}
	}


	private void Update ()
	{
		if (isLocalPlayer)
		{
			if (SceneManager.GetActiveScene ().name == "Simulator")
			{
				float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;
				float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

				transform.Rotate (0, x, 0);
				transform.Translate (0, 0, z);

				transform.position = new Vector3 (transform.position.x, defaultHeight, transform.position.z);
			}
			else
			{
				transform.position = cameraTransform.position;
				transform.rotation = cameraTransform.rotation;
			}
		}
		else
		{
			remotePlayersTransforms.TryGetValue (netId.Value, out targetTransform);
			if (targetTransform != null)
			{
				transform.position = Vector3.Lerp (transform.position, targetTransform.position, smoothing);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetTransform.rotation, smoothing);
			}
		}
	}


	private IEnumerator CommunicateForever()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("transform", SerializableTransform.ToJson (transform));
			WWW postRequest = new WWW(serverUrl + "/" + localPlayerNetId, form);
			yield return postRequest;
			if (string.IsNullOrEmpty (postRequest.error))
			{
				remotePlayersTransforms = SerializableTransform.FromDictJson (postRequest.text);
			}
			else
			{
				Debug.LogError ("Failed to POST to the server: " + postRequest.error);
			}
			yield return new WaitForSeconds (sleepBetweenRequests);
		}
	}
}
