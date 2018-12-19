using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerNetworkTransform : NetworkBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;

    [SyncVar]
    Vector3 leftHandPos;
    [SyncVar]
    Quaternion leftHandRot;
    [SyncVar]
    Vector3 rightHandPos;
    [SyncVar]
    Quaternion rightHandRot;
    [SyncVar]
    Vector3 headPos;
    [SyncVar]
    Quaternion headRot;


	void Update()
    {
		if (isLocalPlayer)
        {
            CmdSetPositionAndRotation(
                leftHand.position,
                leftHand.rotation,
                rightHand.position,
                rightHand.rotation,
                head.position,
                head.rotation
            );
        }
        else
        {
            leftHand.position = leftHandPos;
            leftHand.rotation = leftHandRot;
            rightHand.position = rightHandPos;
            rightHand.rotation = rightHandRot;
            head.position = headPos;
            head.rotation = headRot;
        }
	}


    [Command]
    void CmdSetPositionAndRotation(
        Vector3 lhp,
        Quaternion lhr,
        Vector3 rhp,
        Quaternion rhr,
        Vector3 hp,
        Quaternion hr
    )
    {
        leftHandPos = lhp;
        leftHandRot = lhr;
        rightHandPos = rhp;
        rightHandRot = rhr;
        headPos = hp;
        headRot = hr;
    }
}
