using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour {

	private void Awake()
	{
		Destroy (gameObject, 5f);  // In case it never collides.
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy (gameObject);
	}
}
