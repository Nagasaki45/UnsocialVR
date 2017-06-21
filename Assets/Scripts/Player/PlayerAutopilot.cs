using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : NetworkBehaviour {

	public string controllerTag;

	private SteamVR_TrackedObject trackedObj;
	private NetworkGui networkGui;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int) trackedObj.index); }
	}


	void Start()
	{
		networkGui = GameObject.FindObjectOfType<NetworkGui> ();
		if (isLocalPlayer && SceneManager.GetActiveScene ().name != "Simulator")
		{
			trackedObj = GameObject.FindGameObjectWithTag ("LeftHand").GetComponent<SteamVR_TrackedObject> ();
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
		}
	}


	private IEnumerator StartAutopilot()
	{
		Debug.Log("Player " + netId.Value + " starts autopilot!");
		WWW getRequest = new WWW(networkGui.serversAddress + ":8080/" + netId.Value + "/start_autopilot");
		yield return getRequest;
	}


	private IEnumerator StopAutopilot()
	{
		Debug.Log("Player " + netId.Value + " stops autopilot!");
		WWW getRequest = new WWW(networkGui.serversAddress + ":8080/" + netId.Value + "/stop_autopilot");
		yield return getRequest;
	}
}
