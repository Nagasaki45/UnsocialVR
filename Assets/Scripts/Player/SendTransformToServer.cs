using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendTransformToServer : NetworkBehaviour {

	public string serverUrl;


	private void Start()
	{
		StartCoroutine(SendForever ());
	}


	private IEnumerator SendForever()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("transform", SerializableTransform.ToJson (transform));
			WWW postRequest = new WWW(serverUrl + "/" + netId.Value, form);
			yield return postRequest;
			if (!string.IsNullOrEmpty (postRequest.error))
			{
				Debug.LogError ("Failed to POST to the server: " + postRequest.error);
			}
		}
	}
}
