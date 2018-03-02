﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : MonoBehaviour {

    public string controllerTag;
    public GameObject hiddenPlayerPrefab;
    public bool isFaking = false;

    private GameObject hiddenPlayerObj;
    private TokenSpawner tokenSpawner;
    private FlashScreen flashScreen;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    void Start()
    {
        tokenSpawner = GameObject.FindGameObjectWithTag ("TokenSpawner").GetComponent<TokenSpawner> ();
        flashScreen = GameObject.FindGameObjectWithTag ("FlashScreen").GetComponent<FlashScreen> ();
        if (SceneManager.GetActiveScene ().name != "Simulator")
        {
            trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
        }
    }


    void Update()
    {
        if (SceneManager.GetActiveScene ().name != "Simulator")
        {
            if (Controller.GetHairTriggerDown ())
            {
                StartAutopilot ();
            }
            else if (Controller.GetHairTriggerUp ())
            {
                StopAutopilot ();
            }
        }
        else
        {
            if (Input.GetButtonDown ("Autopilot"))
            {
                StartAutopilot ();
            }
            else if (Input.GetButtonUp ("Autopilot"))
            {
                StopAutopilot ();
            }
        }
    }


    private void StartAutopilot()
    {
        Debug.Log("Local player starts autopilot!");
        isFaking = true;

        // Start spawning tokens
        tokenSpawner.active = true;

        // Flash the screen
        flashScreen.Flash();

        // Turn off body trackers and controllers
        GetComponent<PlayerMovementControl> ().SetControl (false);

        // Turn on faking generators
        SetFakingGenerators(true);

        // Instantiate the hidden player and control it
        hiddenPlayerObj = Instantiate (hiddenPlayerPrefab, transform.position, transform.rotation);
        hiddenPlayerObj.GetComponent<PlayerMovementControl> ().SetControl (true);

        // Hide me
        SetVisibility (false);
    }


    private void StopAutopilot()
    {
        Debug.Log("Local player stops autopilot!");
        isFaking = false;

        // Stop spawning tokens
        tokenSpawner.active = false;

        // Flash the screen
        flashScreen.Flash();

        // Turn off faking generators
        SetFakingGenerators(false);

        // Jump back into the faking avatar
        if (SceneManager.GetActiveScene().name != "Simulator")
        {
            ResetCamera();
        }

        // Turn on body trackers
        GetComponent<PlayerMovementControl> ().SetControl (true);

        // Destroy the hidden player
        Destroy(hiddenPlayerObj);

        // Show me
        SetVisibility (true);
    }


    private void SetVisibility(bool onOff) {
        foreach (var r in gameObject.GetComponentsInChildren<MeshRenderer> ()) {
            r.enabled = onOff;
        }
    }


    private void SetFakingGenerators(bool onOff)
    {
        GetComponent<Notifier>().active = onOff;
        GetComponent<LookAtSpeaker>().active = onOff;

        foreach (var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.active = onOff;
        }
        // TODO more faking generators
    }


    private void ResetCamera()
    {
        // Get the components involved
        Transform cameraRig = GameObject.FindGameObjectWithTag("CameraRig").transform;
        Transform hiddenPlayerHead = hiddenPlayerObj.transform.Find("HeadController");
        Transform playerHead = transform.Find("HeadController");

        // Calculate changes in advance
        float angle = hiddenPlayerHead.rotation.eulerAngles.y - playerHead.rotation.eulerAngles.y;
        Vector3 eulerRotation = new Vector3(0f, -angle, 0f);
        Vector3 relativePositionBeforeRotation = hiddenPlayerHead.position - cameraRig.position;
        Vector3 relativePositionAfterRotation = Quaternion.Euler(eulerRotation) * relativePositionBeforeRotation;
        //                 Shifting to fix the rotation offset                                final shift to move to the player head
        Vector3 rigShift = (relativePositionBeforeRotation - relativePositionAfterRotation) + (playerHead.position - hiddenPlayerHead.position);
        rigShift.y = 0;

        // Apply them all at once
        cameraRig.Rotate(eulerRotation);
        cameraRig.position += rigShift;
    }
}
