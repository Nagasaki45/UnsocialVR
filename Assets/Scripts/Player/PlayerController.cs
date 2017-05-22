using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerController : NetworkBehaviour {

	public static uint localPlayerNetId;
	// public static Dictionary<uint, SerializableTransform> remotePlayersTransforms;

	// Using keyboard in simulator
	public float speed;
	public float angularSpeed;
	public float defaultHeight;

	// Local vs remote players
	public string serverUrl;
	public float smoothing;


	private Transform cameraTransform;
	private SerializableTransform targetTransform;


	private void Start()
	{
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		if (isLocalPlayer)
		{
			localPlayerNetId = netId.Value;
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
			// remotePlayersTransforms.TryGetValue (netId.Value, out targetTransform);
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
			WWW postRequest = new WWW(serverUrl + "/" + netId.Value, form);
			yield return postRequest;
			if (!string.IsNullOrEmpty (postRequest.error))
			{
				Debug.LogError ("Failed to POST to the server: " + postRequest.error);
			}
		}
	}
}
