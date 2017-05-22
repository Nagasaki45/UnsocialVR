using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ReceiveTransformFromServer : NetworkBehaviour {

	public string serverUrl;
	public float smoothing;

	private SerializableTransform targetTransform;


	private void Start()
	{
		StartCoroutine(ReceiveForever ());
	}


	private void Update()
	{
		if (targetTransform != null)
		{
			transform.position = Vector3.Lerp (transform.position, targetTransform.position, smoothing);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetTransform.rotation, smoothing);
		}
	}


	private IEnumerator ReceiveForever()
	{
		while (true)
		{
			WWW getRequest = new WWW(serverUrl + "/" + netId.Value + "/1");  // TODO send local player
			yield return getRequest;
			if (string.IsNullOrEmpty (getRequest.error))
			{
				targetTransform = SerializableTransform.FromJson (getRequest.text);
			}
			else
			{
				Debug.LogError ("Failed to GET to the server: " + getRequest.error);
			}
		}
	}
}