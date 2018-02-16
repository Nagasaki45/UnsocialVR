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
	public Transform headTransform;
	public Transform leftHandTransform;
	public Transform rightHandTransform;
	public string state;

	// Local player settings
	public float sleepBetweenRequests;

	// Remote players settings
	public Color color;
	public float transformSmoothing;
	public float slowSmoothing;
	public float ignoredScale;

	private PlayerTalking playerTalking;
	private Animator animator;


	private void Start()
	{
		playerTalking = GetComponent<PlayerTalking> ();
		animator = GetComponent<Animator> ();
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
				state = received.state;
				if (state == "autopilot")
				{
					uint attention = (uint) received.attention;
					if (attention > 0)
					{
						Vector3 target;
						// Rotate towards the speaker, if it's another remote player that is not the faker
						// Or rotate towards me
						if (remotePlayersData.ContainsKey(attention) && attention != netId.Value)
						{
							target = remotePlayersData [attention].chestPosition;
						}
						else
						{
							target = localPlayerData.chestPosition;
						}
						Quaternion newDir = Quaternion.LookRotation ((target - transform.position).normalized);
						transform.rotation = Quaternion.Slerp (transform.rotation, newDir, Time.deltaTime * slowSmoothing);
					}
					playerTalking.isTalking = false;
				}
				else
				{
					transform.position = Vector3.Lerp (transform.position, received.chestPosition, transformSmoothing);
					transform.rotation = Quaternion.Lerp (transform.rotation, received.chestRotation, transformSmoothing);
					playerTalking.isTalking = received.isTalking;
				}
				UpdateRemotePlayerTransforms (received);
			}
		}
	}


	private void UpdateLocalPlayerTransforms()
	{
		localPlayerData.chestPosition = transform.position;
		localPlayerData.chestRotation = transform.rotation;
		localPlayerData.headPosition = headTransform.localPosition;
		localPlayerData.headRotation = headTransform.localRotation;
		localPlayerData.leftHandPosition = leftHandTransform.localPosition;
		localPlayerData.leftHandRotation = leftHandTransform.localRotation;
		localPlayerData.rightHandPosition = rightHandTransform.localPosition;
		localPlayerData.rightHandRotation = rightHandTransform.localRotation;
		localPlayerData.isTalking = playerTalking.isTalking;
	}


	private void UpdateRemotePlayerTransforms(PlayerData received)
	{
		headTransform.localPosition = Vector3.Lerp (headTransform.localPosition, received.headPosition, transformSmoothing);
		headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, received.headRotation, transformSmoothing);
		leftHandTransform.localPosition = Vector3.Lerp (leftHandTransform.localPosition, received.leftHandPosition, transformSmoothing);
		leftHandTransform.localRotation = Quaternion.Lerp (leftHandTransform.localRotation, received.leftHandRotation, transformSmoothing);
		rightHandTransform.localPosition = Vector3.Lerp (rightHandTransform.localPosition, received.rightHandPosition, transformSmoothing);
		rightHandTransform.localRotation = Quaternion.Lerp (rightHandTransform.localRotation, received.rightHandRotation, transformSmoothing);
		animator.SetBool ("nodding", received.nodding);
	}


	private IEnumerator CommunicateForever()
	{
		// Let the server know my participantId.
		while (true)
		{
			WWW request = new WWW("http://" + NetworkGui.serversAddress + ":8080/" + netId.Value + "/participant-id/" + NetworkGui.participantId);
			yield return request;
			if (string.IsNullOrEmpty (request.error))
			{
				break;
			}
			else
			{
				Debug.LogError ("Failed to GET to the server: " + request.error);
			}
		}


		while (true)
		{
			WWW postRequest = BuildUpdateRequest (netId.Value);
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


	public static WWW BuildUpdateRequest(uint playerId)
	{
		WWWForm form = new WWWForm ();
		form.AddField("transform", localPlayerData.ToJson());
		return new WWW("http://" + NetworkGui.serversAddress + ":8080/" + playerId, form);
	}
}
