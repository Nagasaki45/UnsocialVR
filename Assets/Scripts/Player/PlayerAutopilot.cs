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
					StartCoroutine(StartAutopiloting ());
				}
			}
			else
			{
				if (Input.GetButtonDown ("Autopilot"))
				{
					StartCoroutine(StartAutopiloting ());
				}
			}
		}
	}


	private IEnumerator StartAutopiloting()
	{
		Debug.Log("Player " + netId.Value + " starts autopiloting!");
		WWW getRequest = new WWW(networkGui.serversAddress + ":8080/" + netId.Value + "/start_autopilot");
		yield return getRequest;
		Debug.Log (getRequest.text);
	}
}
