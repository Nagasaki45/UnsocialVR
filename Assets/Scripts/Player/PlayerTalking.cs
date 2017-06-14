using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour, IVoiceActivationListener {

	public Transform mouth;
	public float mouthClose;
	public float mouthOpen;
	public float speed;
	public bool isTalking;

	private float epsilon = 0.01f;
	private DissonanceComms comms;
	private float target;


	private void Start()
	{
		target = mouthClose;
		comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();
		if (isLocalPlayer)
		{
			comms.SubcribeToVoiceActivation (this);
		}
	}


	public void VoiceActivationStart()
	{
		isTalking = true;
	}


	public void VoiceActivationStop()
	{
		isTalking = false;
	}


	private void Update ()
	{
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
	}
}
