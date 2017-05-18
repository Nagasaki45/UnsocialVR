using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Autopilot : NetworkBehaviour {

	public const short TRANSFORM_MESSAGE_TYPE = MsgType.Highest + 1;

	public class TransformMessage : MessageBase {
		public uint netId;
		public Vector3 position;
		public Quaternion rotation;
	}


	public float smoothing;


	////////////
	// Client //
	////////////

	private NetworkClient networkClient;
	private TransformMessage targetTransform;


	private void Start()
	{
		if (!isLocalPlayer)
		{
			networkClient = new NetworkClient ();
			networkClient.RegisterHandler (TRANSFORM_MESSAGE_TYPE, OnTransform);
			networkClient.Connect (NetworkManager.singleton.networkAddress, NetworkManager.singleton.networkPort);
		}
	}


	public void OnTransform(NetworkMessage netMsg)
	{
		var receivedTransform = netMsg.ReadMessage<TransformMessage> ();
		if (receivedTransform.netId == netId.Value)
		{
			targetTransform = receivedTransform;
		}
	}


	private void Update()
	{
		if (isLocalPlayer)
		{
			CmdSendTransformToServer (transform.position, transform.rotation);
		}
		else if (targetTransform != null)
		{
			transform.position = Vector3.Lerp (transform.position, targetTransform.position, smoothing);
			transform.rotation = Quaternion.Lerp (transform.rotation, targetTransform.rotation, smoothing);
		}
	}


	////////////
	// Server //
	////////////

	[Command]
	private void CmdSendTransformToServer(Vector3 position, Quaternion rotation)
	{
		var transformMessage = new TransformMessage ();
		transformMessage.netId = netId.Value;
		transformMessage.position = position;
		transformMessage.rotation = rotation;
		NetworkServer.SendToAll (TRANSFORM_MESSAGE_TYPE, transformMessage);
	}
}
