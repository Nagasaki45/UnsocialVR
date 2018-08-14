using System.Collections.Generic;
using UnityEngine;


public class Recenter : MonoBehaviour {

    public float radious;

    GameObject me;
    Transform cameraRig;
    Vector3 position;
    Quaternion rotation;


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
            List<Vector3> occupiedPositions = GetOccupiedPositions();
            FindEmptyChair(occupiedPositions);  // Sets the global position/rotation
            ResetCamera(position, rotation);
        }
    }


    List<Vector3> GetOccupiedPositions()
    {
        List<Vector3> occupiedPositions = new List<Vector3>();
        foreach (var otherPlayer in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (otherPlayer != me)
            {
                Transform otherPlayerHead = otherPlayer.transform.Find("Performative/Head");
                occupiedPositions.Add(otherPlayerHead.position);
            }
        }
        return occupiedPositions;
    }


    void FindEmptyChair(List<Vector3> occupiedPositions)
    {
        for (int i = 0; i < 3; i++)
        {
            position = Quaternion.Euler(0, 120 * i, 0) * Vector3.right * radious;
            rotation = Quaternion.Euler(0, 120 * i - 90, 0);
            bool emptyChair = true;
            foreach (var occupiedPosition in occupiedPositions)
            {
                if (Vector3.Distance(position, occupiedPosition) < 1)
                {
                    emptyChair = false;
                    break;
                }
            }
            if (emptyChair)
            {
                break;
            }
        }
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
