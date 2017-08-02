using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSpawner : MonoBehaviour {

	public GameObject tokenPrefab;
	public Transform[] spawnPoints;
	public float spawnTime;
	public bool isSpawning = false;


	private void Start ()
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	

	private void Spawn()
	{
		if (isSpawning)
		{
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints [spawnPointIndex];
			Instantiate (tokenPrefab, spawnPoint.position, spawnPoint.rotation);
		}
	}
}
