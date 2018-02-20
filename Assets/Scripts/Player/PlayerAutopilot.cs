using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : NetworkBehaviour {

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
        if (isLocalPlayer)
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
    }


    private void StartAutopilot()
    {
        Debug.Log("Local player starts autopilot!");
        isFaking = true;

        // Start spawning tokens
        tokenSpawner.enabled = true;

        // Flash the screen
        flashScreen.Flash();

        // Turn off body trackers and controllers
        GetComponent<PlayerMovementControl> ().SetControl (false);

        // TODO Turn on faking generators

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
        tokenSpawner.enabled = false;

        // Flash the screen
        flashScreen.Flash();

        // TODO Turn off faking generators

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
}
