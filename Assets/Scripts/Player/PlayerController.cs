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
					UpdateRemotePlayerTransforms (received, false);
					playerTalking.isTalking = received.isTalking;
					SetChildrenRenderersEnabledState (true);
				}
				else if (received.state == "autopilot")
				{
					UpdateRemotePlayerTransforms (received, true);
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
		localPlayerData.leftHandPosition = leftHandTransform.localPosition;
		localPlayerData.leftHandRotation = leftHandTransform.localRotation;
		localPlayerData.rightHandPosition = rightHandTransform.localPosition;
		localPlayerData.rightHandRotation = rightHandTransform.localRotation;
	}


	private void UpdateRemotePlayerTransforms(PlayerData received, bool autopilotRotation)
	{
		transform.position = Vector3.Lerp (transform.position, received.headPosition, transformSmoothing);
		if (autopilotRotation)
		{
			// TODO change to chest position
			transform.rotation = Quaternion.LookRotation (localPlayerData.headPosition - transform.position);
		}
		else
		{
			transform.rotation = Quaternion.Lerp (transform.rotation, received.headRotation, transformSmoothing);
		}
		leftHandTransform.localPosition = Vector3.Lerp (leftHandTransform.localPosition, received.leftHandPosition, transformSmoothing);
		leftHandTransform.localRotation = Quaternion.Lerp (leftHandTransform.localRotation, received.leftHandRotation, transformSmoothing);
		rightHandTransform.localPosition = Vector3.Lerp (rightHandTransform.localPosition, received.rightHandPosition, transformSmoothing);
		rightHandTransform.localRotation = Quaternion.Lerp (rightHandTransform.localRotation, received.rightHandRotation, transformSmoothing);
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
