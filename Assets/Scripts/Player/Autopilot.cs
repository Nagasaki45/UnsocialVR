using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Autopilot : NetworkBehaviour {

	public string serverUrl;
	public float smoothing;

	private Vector3 targetPosition;
	private Quaternion targetRotation;

	private void Start ()
	{
		if (isLocalPlayer)
			StartCoroutine(SendTransformToServer ());
		else
			StartUpdateTransformFromServer ();
	}


	private void Update ()
	{
		if (isLocalPlayer)
			return;

		transform.position = Vector3.Lerp (transform.position, targetPosition, smoothing);
		transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, smoothing);
	}
		

	private IEnumerator SendTransformToServer()
	{
		while (true)
		{
			WWWForm form = new WWWForm ();
			form.AddField("position", JsonUtility.ToJson (transform.position));
			form.AddField("rotation", JsonUtility.ToJson (transform.rotation));
			WWW postRequest = new WWW(serverUrl + netId, form);
			yield return postRequest;
		}
	}


	private void StartUpdateTransformFromServer()
	{
		StartCoroutine(UpdatePositionFromServer ());
		StartCoroutine(UpdateRotationFromServer ());
	}


	private IEnumerator UpdatePositionFromServer()
	{
		while (true)
		{
			WWW positionGetRequest = new WWW(serverUrl + netId + "/position");
			yield return positionGetRequest;
			targetPosition = JsonUtility.FromJson<Vector3> (positionGetRequest.text);
		}
	}


	private IEnumerator UpdateRotationFromServer()
	{
		while (true)
		{
			WWW rotationGetRequest = new WWW(serverUrl + netId + "/rotation");
			yield return rotationGetRequest;
			targetRotation = JsonUtility.FromJson<Quaternion> (rotationGetRequest.text);
		}
	}
}
