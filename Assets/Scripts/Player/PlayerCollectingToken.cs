using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectingToken : MonoBehaviour {

	private AudioSource collectSound;


	private void Start()
	{
		collectSound = GetComponent<AudioSource> ();
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Token")
		{
			Destroy (other.gameObject);
			collectSound.Play ();

			// Send the message to server
			StartCoroutine (SendTokenCollectionToServer ());
		}
	}

	private IEnumerator SendTokenCollectionToServer()
	{
		uint player_id = GetComponentInParent<PlayerController> ().netId.Value;
		yield return new WWW(NetworkGui.serversAddress + ":8080/" + player_id + "/collect-token");
	}
}
