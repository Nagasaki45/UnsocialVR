using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Autopilot : NetworkBehaviour {

	public static GameObject localPlayer;
	[SyncVar]
	public bool isOn = false;

	private bool previousIsOn = false;


	// Caching the local player
	public override void OnStartLocalPlayer()
	{
		localPlayer = gameObject;
	}


	// Update is called once per frame
	void Update ()
	{
		if (isLocalPlayer && Input.GetButtonUp ("Autopilot"))
			CmdToggleAutopilot ();

		if (!isLocalPlayer)
		{
			if (!previousIsOn && isOn)
			{
				AutopilotSetup ();
			}
			else if (previousIsOn && isOn)
			{
				AutopilotUpdate ();
			}
			else if (previousIsOn && !isOn)
			{
				AutopilotTeardown();
			}
		}

		previousIsOn = isOn;
	}


	[Command]
	void CmdToggleAutopilot()
	{
		isOn = !isOn;
	}


	void AutopilotSetup()
	{
		GetComponentInChildren<MeshRenderer> ().material.color = Color.red;
	}


	void AutopilotTeardown()
	{
		GetComponentInChildren<MeshRenderer> ().material.color = Color.white;
	}


	void AutopilotUpdate()
	{
		transform.position = Vector3.zero;
	}
}
