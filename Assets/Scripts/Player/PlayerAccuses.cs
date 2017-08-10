using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerAccuses : NetworkBehaviour {

	public string controllerTag;
	public AudioClip accusingNothing;
	public AudioClip accusingPlayer;

	private PlayerApproaches playerApproaches;
	private SteamVR_TrackedObject trackedObj;
	private AudioSource audioSource;

	private SteamVR_Controller.Device Controller
	{
		get { return SteamVR_Controller.Input((int) trackedObj.index); }
	}


	private void Awake()
	{
		audioSource = GetComponent<AudioSource> ();
	}


	private void Start ()
	{
		playerApproaches = GetComponent<PlayerApproaches> ();
		if (SceneManager.GetActiveScene ().name != "Simulator")
		{
			trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
		}
	}


	private void Update () {
		if (isLocalPlayer)
		{
			if (SceneManager.GetActiveScene ().name == "Simulator" && Input.GetButtonDown ("Accuse"))
			{
				StartCoroutine (AccusePlayerForAutopiloting (playerApproaches.attention));
			}
			else if (Controller.GetHairTriggerDown ())
			{
				StartCoroutine (AccusePlayerForAutopiloting (playerApproaches.attention));
			}
		}
	}


	private IEnumerator AccusePlayerForAutopiloting(PlayerController otherPlayer)
	{
		if (null == otherPlayer)
		{
			audioSource.clip = accusingNothing;
		}
		else
		{
			audioSource.clip = accusingPlayer;

			bool correct = (otherPlayer.state == "autopilot");
			string prefix = correct ? "Rightfully" : "Mistakenly";
			Debug.Log (prefix + " accusing " + otherPlayer.netId.Value + " for autopiloting");

			// Send the message to the server that I'm not autopiloting anymore
			string conclusion = correct ? "correct" : "incorrect";
			yield return new WWW(NetworkGui.serversAddress + ":8080/" + netId.Value + "/accuse/" + otherPlayer.netId.Value + "/" + conclusion);
		}

		audioSource.Play ();
	}
}
