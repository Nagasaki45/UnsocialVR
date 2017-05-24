using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerController : NetworkBehaviour {

	// Keep local player info as static
	// One instance of the class per app instance will work
	public static Transform localPlayerTransform;
	public static uint localPlayerNetId;
	public static Dictionary<uint, SerializableTransform> remotePlayersTransforms;

	// Local player settings
	public string serverUrl;
	public float sleepBetweenRequests;

	// Remote players settings
	public Color color;
	public float transformSmoothing;
	public float autopilotSmoothing;

	// Using keyboard in simulator
	public float speed;
	public float angularSpeed;
	public float defaultHeight;

	private PlayerTalking playerTalking;
	private PlayerAttention playerAttention;
	private Transform cameraTransform;
	private SerializableTransform targetTransform;


	private void Start()
	{
		playerTalking = GetComponent<PlayerTalking> ();
		playerAttention = GetComponent<PlayerAttention> ();
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
			localPlayerTransform = transform;
			if (SceneManager.GetActiveScene ().name == "Simulator")
			{
				float x = Input.GetAxis ("Horizontal") * angularSpeed * Time.deltaTime;
				float z = Input.GetAxis ("Vertical") * speed * Time.deltaTime;

				transform.Rotate (0, x, 0);
				transform.Translate (0, 0, z);

				transform.position = new Vector3 (transform.position.x, defaultHeight, transform.position.z);

				// Overcomplicated to allow manual switching of isTalking for debuging
				if (Input.GetButtonDown ("Talk"))
					playerTalking.isTalking = true;
				else if (Input.GetButtonUp ("Talk"))
					playerTalking.isTalking = false;

			}
			else
			{
				transform.position = cameraTransform.position;
				transform.rotation = cameraTransform.rotation;

				// TODO Set talking state for VR
			}
		}
		else
		{
			remotePlayersTransforms.TryGetValue (netId.Value, out targetTransform);
			if (targetTransform != null)
			{
				Debug.Log (netId.Value + " state: " + targetTransform.state);
				if (targetTransform.state == "real")
				{
					transform.position = Vector3.Lerp (transform.position, targetTransform.position, transformSmoothing);
					transform.rotation = Quaternion.Lerp (transform.rotation, targetTransform.rotation, transformSmoothing);
					playerTalking.isTalking = targetTransform.isTalking;
				}
				else if (targetTransform.state == "autopilot")
				{
					playerTalking.isTalking = false;
					Vector3 targetDir = localPlayerTransform.position - transform.position;
					Vector3 fakedRotation = Vector3.RotateTowards (transform.forward, targetDir, autopilotSmoothing, 0.0f);
					transform.rotation = Quaternion.LookRotation (fakedRotation);
				}
				else if (targetTransform.state == "ignored")
				{
					playerTalking.isTalking = false;
				}
			}
		}
	}


	private IEnumerator CommunicateForever()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("transform", SerializableTransform.ToJson (transform, playerTalking.isTalking, playerAttention.attentionTo));
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
