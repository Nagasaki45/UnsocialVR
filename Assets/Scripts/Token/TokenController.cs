using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenController : MonoBehaviour {

	public float livingTime;

	private float startTime;


	private void Awake()
	{
		startTime = Time.time;
		Object.Destroy (gameObject, livingTime);
	}


	private void Update ()
	{
		float scale = 1 - (Time.time - startTime) / livingTime;
		transform.localScale = scale * Vector3.one;
	}
}
