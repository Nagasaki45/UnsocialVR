using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour {

	public Transform mouth;
	public float mouthClose;
	public float mouthOpen;
	public float speed;
	public bool isTalking;
	public float talkingAmplitudeThreshold;

	private bool previousIsTalking = false;
	private float epsilon = 0.01f;
	private DissonanceComms comms;
	private float target;


	private void Start()
	{
		target = mouthClose;
		comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();
	}


	private void Update ()
	{
		if (isLocalPlayer)
		{
			if (SceneManager.GetActiveScene ().name == "Simulator")
			{
				if (Input.GetButtonUp ("Talk"))
				{
					isTalking = !isTalking;
				}
			}
			else
			{
				foreach (VoicePlayerState voicePlayerState in comms.Players)
				{
					// UglyHack (c) to check the type of a private class.
					if (voicePlayerState.GetType ().Name == "LocalVoicePlayerState")
					{
						isTalking = IsTalking (voicePlayerState);
						break;
					}
				}
			}

			SendStartStopTalkingToServer ();
		}

		if (isTalking)
		{
			if (mouth.localScale.y > mouthOpen - epsilon)
			{
				target = mouthClose;
			}
			else if (mouth.localScale.y < mouthClose + epsilon)
			{
				target = mouthOpen;
			}
		}
		else
		{
			target = mouthClose;
		}
		Vector3 targetVector = new Vector3 (mouth.localScale.x, target, mouth.localScale.z);
		mouth.localScale = Vector3.Lerp (mouth.localScale, targetVector, speed * Time.deltaTime);

		previousIsTalking = isTalking;
	}


	private bool IsTalking(VoicePlayerState state)
	{
		return state.IsSpeaking && (state.Amplitude > talkingAmplitudeThreshold);
	}


	private void SendStartStopTalkingToServer()
	{
		if (isTalking != previousIsTalking)
		{
			string msg = isTalking ? "start" : "stop";
			StartCoroutine (SendTalkingToServer (msg));
		}
	}


	private IEnumerator SendTalkingToServer(string msg)
	{
		Debug.Log("Local player " + msg + "s talking!");
		yield return new WWW(NetworkGui.serversAddress + ":8080/" + netId.Value + "/talking/" + msg);
	}
}
