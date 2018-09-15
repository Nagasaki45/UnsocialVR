using System;
using System.Collections.Generic;
using UnityEngine;


public class Recenter : MonoBehaviour {

    public float radious;

    GameObject me;
    Transform cameraRig;


    void Start()
    {
        me = transform.parent.parent.gameObject;  // Player/Self/Head
        cameraRig = GameObject.FindGameObjectWithTag("CameraRig").transform;
    }


    void Update()
    {
        if (Input.GetButtonDown("Recenter"))
        {
            Logger.Event("Recentering");
            List<int> occupiedChairs = GetOccupiedChairs();
            int chair = FindEmptyChair(occupiedChairs);
            me.GetComponent<PlayerState>().CmdSetChair(chair);
            Vector3 position = Quaternion.Euler(0, 120 * chair, 0) * Vector3.right * radious;
            Quaternion rotation = Quaternion.Euler(0, 120 * chair - 90, 0);
            ResetCamera(position, rotation);
        }
    }


    List<int> GetOccupiedChairs()
    {
        List<int> occupiedChairs = new List<int>();
        foreach (var otherPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (otherPlayer != me)
            {
                occupiedChairs.Add(otherPlayer.GetComponent<PlayerState>().GetChair());
            }
        }
        return occupiedChairs;
    }

    int FindEmptyChair(List<int> occupiedChairs)
    {
        for (int i = 0; i < 3; i++)
        {
            if (!occupiedChairs.Contains(i)) {
                return i;
            }
        }
        return 0;  // This should never happen, but throwing an exception is just
                   // too complicated in c# :(
    }


    void ResetCamera(Vector3 targetPos, Quaternion targetRot)
    {
        // Calculate changes in advance
        float angle = transform.rotation.eulerAngles.y - targetRot.eulerAngles.y;
        Vector3 eulerRotation = new Vector3(0f, -angle, 0f);
        Vector3 relativePositionBeforeRotation = transform.position - cameraRig.position;
        Vector3 relativePositionAfterRotation = Quaternion.Euler(eulerRotation) * relativePositionBeforeRotation;
        //                 Shifting to fix the rotation offset                                final shift to move to the player head
        Vector3 rigShift = (relativePositionBeforeRotation - relativePositionAfterRotation) + (targetPos - transform.position);
        rigShift.y = 0;

        // Apply them all at once
        cameraRig.Rotate(eulerRotation);
        cameraRig.position += rigShift;
    }
}
