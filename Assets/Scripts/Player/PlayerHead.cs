using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerHead : NetworkBehaviour
{
    [SyncVar]
    Vector3 position;
    [SyncVar]
    Quaternion rotation;

    Transform headTransform;


	void Start()
    {
        headTransform = transform.Find(isLocalPlayer ? "Local/Head" : "Remote/Head");
	}


	void Update()
    {
        // Local player updates the position / rotation from the transform
		if (isLocalPlayer)
        {
            CmdSetPositionAndRotation(headTransform.position, headTransform.rotation);
        }
        // Remote player set the position / rotation to the transform
        else
        {
            headTransform.position = position;
            headTransform.rotation = rotation;
        }
	}


    [Command]
    void CmdSetPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        position = newPosition;
        rotation = newRotation;
    }
}
