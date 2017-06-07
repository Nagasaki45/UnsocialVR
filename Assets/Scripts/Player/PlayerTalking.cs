using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerTalking : NetworkBehaviour {

	public GameObject mouth;
	public AudioSource audioSource;
	public float mouthClose;
	public float mouthOpen;
	public float speed;
	public bool isTalking;

	private Vector3 target;


	private void Awake()
	{
		target = CreateTarget (mouthOpen);
	}


	private Vector3 CreateTarget(float position)
	{
		return new Vector3 (mouth.transform.localScale.x, position, mouth.transform.localScale.z);
	}


	private void Update ()
	{
		if (isLocalPlayer)
		{
			// Overcomplicated to allow manual switching of isTalking for debuging
			if (Input.GetButtonDown ("Talk"))
				isTalking = true;
			else if (Input.GetButtonUp ("Talk"))
				isTalking = false;

			PlayerController.localPlayerData.isTalking = isTalking;
		}

		if (isTalking)
		{
			audioSource.volume = 1f;
			if (Vector3.Distance (mouth.transform.localScale, target) < 0.01f)
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
			mouth.transform.localScale = Vector3.Lerp (mouth.transform.localScale, target, speed * Time.deltaTime);
		}
		else
		{
			audioSource.volume = 0f;
			mouth.transform.localScale = CreateTarget (mouthClose);
		}
	}
}
