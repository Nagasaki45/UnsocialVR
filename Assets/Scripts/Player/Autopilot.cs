using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Autopilot : NetworkBehaviour {

	public string serverUrl;
	public float smoothing;
	[HideInInspector] public static uint localPlayerId;

	private SerializableTransform targetTransform;


	public override void OnStartLocalPlayer()
	{
		localPlayerId = netId.Value;
	}


	private void Start()
	{
		if (isLocalPlayer)
			StartCoroutine(SendTransformToServer ());
		else
			StartCoroutine(UpdateTransformFromServer ());
	}


	private void Update()
	{
		if (isLocalPlayer)
			return;

		if (targetTransform != null)
		{
			transform.position = Vector3.Lerp (transform.position, targetTransform.position, smoothing);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetTransform.rotation, smoothing);
		}
	}
		

	private IEnumerator SendTransformToServer()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("transform", SerializableTransform.ToJson (transform));
			WWW postRequest = new WWW(serverUrl + "/" + netId.Value, form);
			yield return postRequest;
		}
	}


	private IEnumerator UpdateTransformFromServer()
	{
		while (true)
		{
			WWW getRequest = new WWW(serverUrl + "/" + netId.Value + "/" + localPlayerId);
			yield return getRequest;
			if (string.IsNullOrEmpty (getRequest.error))
			{
				targetTransform = SerializableTransform.FromJson (getRequest.text);
			}
		}
	}
}
