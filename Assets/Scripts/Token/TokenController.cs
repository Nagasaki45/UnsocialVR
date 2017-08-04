using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TokenController : MonoBehaviour {

	public float livingTime;

	private float startTime;


	private void Awake()
	{
		startTime = Time.time;
		Object.Destroy (gameObject, livingTime);
	}


	private void Start()
	{
		// In the simulator the collision radius should be much larger
		// because players can't move their hands.
		if (SceneManager.GetActiveScene ().name == "Simulator")
		{
			GetComponent<SphereCollider> ().radius = 1f;
		}
	}


	private void Update ()
	{
		float scale = 1 - (Time.time - startTime) / livingTime;
		transform.localScale = scale * Vector3.one;
	}
}
