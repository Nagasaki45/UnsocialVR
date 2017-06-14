using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour, IVoiceActivationListener {

	public GameObject mouth;
	public float mouthClose;
	public float mouthOpen;
	public float speed;
	public bool isTalking;

	private DissonanceComms comms;
	private Vector3 target;


	private void Awake()
	{
		comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();
	}


	private void Start()
	{
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
			if (null != target && Vector3.Distance (mouth.transform.localScale, target) < 0.01f)
			{
				if (target.y == mouthOpen)
				{
					target = CreateTarget (mouthClose);
				}
				else
				{
					target = CreateTarget (mouthOpen);
				}
			}
		}
		else
		{
			target = CreateTarget (mouthClose);
		}
		mouth.transform.localScale = Vector3.Lerp (mouth.transform.localScale, target, speed * Time.deltaTime);
	}


	private Vector3 CreateTarget(float position)
	{
		return new Vector3 (mouth.transform.localScale.x, position, mouth.transform.localScale.z);
	}
}
