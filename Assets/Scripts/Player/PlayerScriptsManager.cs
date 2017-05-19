using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerScriptsManager : NetworkBehaviour {

	public string serverUrl;
	public Color color;
	public float smoothing;
	public float speed;
	public float angularSpeed;
	public float defaultHeight;


	void Start () {
		string scene = SceneManager.GetActiveScene ().name;

		Debug.Log ("Local: " + isLocalPlayer);
		Debug.Log ("Scene: " + scene);
		Debug.Log ("NetID: " + netId);

		if (isLocalPlayer)
		{
			LocalPlayerColor localPlayerColor = gameObject.AddComponent<LocalPlayerColor> ();
			localPlayerColor.color = color;

			SendTransformToServer sendTransformToServer = gameObject.AddComponent<SendTransformToServer> ();
			sendTransformToServer.serverUrl = serverUrl;

			if (scene == "Simulator") {
				SimulatorPlayerMovement simulatorPlayerMovement = gameObject.AddComponent<SimulatorPlayerMovement> ();
				simulatorPlayerMovement.speed = speed;
				simulatorPlayerMovement.angularSpeed = angularSpeed;
				simulatorPlayerMovement.defaultHeight = defaultHeight;
			} else {
				gameObject.AddComponent<PlayerMovement> ();
			}
		}
		else
		{
			ReceiveTransformFromServer receiveTransformFromServer = gameObject.AddComponent<ReceiveTransformFromServer> ();
			receiveTransformFromServer.serverUrl = serverUrl;
			receiveTransformFromServer.smoothing = smoothing;
		}
	}
}
