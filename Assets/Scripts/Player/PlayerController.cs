using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerController : NetworkBehaviour {

	// Keep local player info as static
	// One instance of the class per app instance will work
	public static PlayerData localPlayerData;
	public static Dictionary<uint, PlayerData> remotePlayersData;
	public Transform leftHandTransform;
	public Transform rightHandTransform;

	// Local player settings
	public string serverUrl;
	public float sleepBetweenRequests;

	// Remote players settings
	public Color color;
	public float transformSmoothing;
	public float autopilotSmoothing;

	private PlayerTalking playerTalking;


	private void Start()
	{
		playerTalking = GetComponent<PlayerTalking> ();
		if (isLocalPlayer)
		{
			localPlayerData = new PlayerData ();
			GetComponentInChildren<MeshRenderer> ().material.color = color;
			StartCoroutine(CommunicateForever ());
		}
	}


	private void Update ()
	{
		if (isLocalPlayer)
		{
			UpdateLocalPlayerTransforms ();
		}
		else if (null != remotePlayersData)
		{
			PlayerData received;
			remotePlayersData.TryGetValue (netId.Value, out received);
			if (null != received)
			{
				Debug.Log (netId.Value + " state: " + received.state);
				if (received.state == "real")
				{
					UpdateRemotePlayerTransforms (received, transformSmoothing);
					playerTalking.isTalking = received.isTalking;
					SetChildrenRenderersEnabledState (true);
				}
				else if (received.state == "autopilot")
				{
					UpdateRemotePlayerTransforms (received, 0f);

					// Rotate towards local player
					Vector3 targetDir = localPlayerData.headPosition - transform.position;  // TODO change to chest position
					Vector3 fakedRotation = Vector3.RotateTowards (transform.forward, targetDir, autopilotSmoothing, 0.0f);
					transform.rotation = Quaternion.LookRotation (fakedRotation);

					playerTalking.isTalking = false;
					SetChildrenRenderersEnabledState (true);
				}
				else if (received.state == "ignored")
				{
					playerTalking.isTalking = false;
					SetChildrenRenderersEnabledState (false);
				}
			}
		}
	}


	private void SetChildrenRenderersEnabledState(bool state)
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.enabled = state;
		}
	}


	private void UpdateLocalPlayerTransforms()
	{
		localPlayerData.headPosition = transform.position;
		localPlayerData.headRotation = transform.rotation;
		localPlayerData.leftHandPosition = leftHandTransform.position;
		localPlayerData.leftHandRotation = leftHandTransform.rotation;
		localPlayerData.rightHandPosition = rightHandTransform.position;
		localPlayerData.rightHandRotation = rightHandTransform.rotation;
	}


	private void UpdateRemotePlayerTransforms(PlayerData received, float smoothing)
	{
		transform.position = Vector3.Lerp (transform.position, received.headPosition, smoothing);
		transform.rotation = Quaternion.Lerp (transform.rotation, received.headRotation, smoothing);
		leftHandTransform.position = Vector3.Lerp (leftHandTransform.position, received.leftHandPosition, smoothing);
		leftHandTransform.rotation = Quaternion.Lerp (leftHandTransform.rotation, received.leftHandRotation, smoothing);
		rightHandTransform.position = Vector3.Lerp (rightHandTransform.position, received.rightHandPosition, smoothing);
		rightHandTransform.rotation = Quaternion.Lerp (rightHandTransform.rotation, received.rightHandRotation, smoothing);
	}


	private IEnumerator CommunicateForever()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("transform", localPlayerData.ToJson());
			WWW postRequest = new WWW(serverUrl + "/" + netId.Value, form);
			yield return postRequest;
			if (string.IsNullOrEmpty (postRequest.error))
			{
				remotePlayersData = PlayerData.FromDictJson (postRequest.text);
			}
			else
			{
				Debug.LogError ("Failed to POST to the server: " + postRequest.error);
			}
			yield return new WaitForSeconds (sleepBetweenRequests);
		}
	}
}
