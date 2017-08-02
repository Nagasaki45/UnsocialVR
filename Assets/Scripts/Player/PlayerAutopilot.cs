using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : NetworkBehaviour {

	public GameObject autopilotMarkerPrefab;
	public string controllerTag;

	private TokenSpawner tokenSpawner;
	private GameObject autopilotMarker;
	private SteamVR_TrackedObject trackedObj;
	private FlashScreen flashScreen;
	private Transform cameraRig;
	private float autopilotXPosition;
	private float autopilotZPosition;
	private float autopilotYRotation;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int) trackedObj.index); }
	}


	void Start()
	{
		tokenSpawner = GameObject.FindGameObjectWithTag ("TokenSpawner").GetComponent<TokenSpawner> ();
		flashScreen = GameObject.FindGameObjectWithTag ("FlashScreen").GetComponent<FlashScreen> ();
		if (SceneManager.GetActiveScene ().name != "Simulator")
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
				if (Input.GetButtonUp ("Autopilot"))
				{
					if (null == autopilotMarker)
					{
						StartCoroutine (StartAutopilot ());
					}
					else
					{
						StartCoroutine (StopAutopilot ());
					}
				}
			}
		}
	}


	private IEnumerator StartAutopilot()
	{
		Debug.Log("Player " + netId.Value + " starts autopilot!");

		// Start spawning tokens
		tokenSpawner.isSpawning = true;

		// Spawn the marker
		autopilotMarker = Instantiate(autopilotMarkerPrefab, transform.position, transform.rotation);

		// Flash the screen
		flashScreen.Flash();

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

		// Stop spawning tokens
		tokenSpawner.isSpawning = false;

		// Delete the marker
		if (null != autopilotMarker)
		{
			Destroy (autopilotMarker);
		}

		// Flash the screen
		flashScreen.Flash();

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
