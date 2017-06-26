﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : NetworkBehaviour {

	public string controllerTag;
	public Color flashScreenIdleColor;
	public Color flashScreenFlashingColor;
	public float flashSpeed;

	private SteamVR_TrackedObject trackedObj;
	private Transform cameraRig;
	private Image flashScreen;
	private float autopilotXPosition;
	private float autopilotZPosition;
	private float autopilotYRotation;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int) trackedObj.index); }
	}


	void Start()
	{
		flashScreen = GameObject.FindObjectOfType<Canvas> ().GetComponentInChildren<Image> ();
		if (isLocalPlayer && SceneManager.GetActiveScene ().name != "Simulator")
		{
			trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
			cameraRig = GameObject.FindGameObjectWithTag ("CameraRig").GetComponent<Transform> ();
		}
	}


	void Update()
	{
		if (isLocalPlayer)
		{
			if (SceneManager.GetActiveScene ().name != "Simulator")
			{
				if (Controller.GetHairTriggerDown ())
				{
					StartCoroutine (StartAutopilot ());
				}
				else if (Controller.GetHairTriggerUp ())
				{
					StartCoroutine (StopAutopilot ());
				}
			}
			else
			{
				if (Input.GetButtonDown ("Autopilot"))
				{
					StartCoroutine(StartAutopilot ());
				}
				else if (Input.GetButtonUp ("Autopilot"))
				{
					StartCoroutine(StopAutopilot ());
				}
			}

			flashScreen.color = Color.Lerp (flashScreen.color, flashScreenIdleColor, flashSpeed * Time.deltaTime);
		}
	}


	private IEnumerator StartAutopilot()
	{
		Debug.Log("Player " + netId.Value + " starts autopilot!");

		// Flash the screen
		flashScreen.color = flashScreenFlashingColor;

		// Keep the rotation and position for later recovery
		autopilotXPosition = transform.position.x;
		autopilotZPosition = transform.position.z;
		autopilotYRotation = transform.rotation.eulerAngles.y;

		// Send the message to the server
		yield return new WWW(NetworkGui.serversAddress + ":8080/" + netId.Value + "/start_autopilot");
	}


	private IEnumerator StopAutopilot()
	{
		Debug.Log("Player " + netId.Value + " stops autopilot!");

		// Flash the screen
		flashScreen.color = flashScreenFlashingColor;

		// Restore transform
		if (SceneManager.GetActiveScene ().name != "Simulator")
		{
			Vector3 positionBeforeRotation = transform.position;
			cameraRig.Rotate(new Vector3(0f, autopilotYRotation - transform.rotation.eulerAngles.y, 0f));
			yield return 0;  // Wait for 1 frame for the transform to update.
			Vector3 positionAfterRotation = transform.position;
			cameraRig.position += positionBeforeRotation - positionAfterRotation;
			yield return 0;  // Wait for 1 frame for the transform to update.

			cameraRig.position += new Vector3(autopilotXPosition - transform.position.x, 0f, autopilotZPosition - transform.position.z);
		}
		else
		{
			transform.rotation = Quaternion.Euler(new Vector3(0f, autopilotYRotation, 0f));
			transform.position = new Vector3(autopilotXPosition, transform.position.y, autopilotZPosition);
		}

		PlayerController.localPlayerData.chestPosition = transform.position;
		PlayerController.localPlayerData.chestRotation = transform.rotation;

		// Update the server with my transform change
		yield return PlayerController.BuildUpdateRequest(netId.Value);

		// Send the message to the server that I'm not autopiloting anymore
		yield return new WWW(NetworkGui.serversAddress + ":8080/" + netId.Value + "/stop_autopilot");
	}

}
