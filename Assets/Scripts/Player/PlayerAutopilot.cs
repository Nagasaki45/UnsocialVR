using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : MonoBehaviour {

    public string controllerTag;
    public string[] fakingTheories;
    public GameObject hiddenPlayerPrefab;

    private PlayerState playerState;
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
        playerState = GetComponent<PlayerState>();
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
            if (Controller.GetHairTriggerDown())
            {
                int i = Random.Range(0, fakingTheories.Length);
                StartAutopilot(fakingTheories[i]);
            }
            else if (Controller.GetHairTriggerUp())
            {
                StopAutopilot();
            }
        }
        else
        {
            foreach (string theory in fakingTheories)
            {
                string input = "Fake" + char.ToUpper(theory[0]) + theory.Substring(1);
                if (Input.GetButtonDown(input))
                {
                    if (playerState.IsFaking())
                    {
                        StopAutopilot();
                    }
                    else
                    {
                        StartAutopilot(theory);
                    }
                }
            }
        }
    }


    void OnDestroy()
    {
        if (hiddenPlayerObj != null)
        {
            Destroy(hiddenPlayerObj);
        }
    }


    void StartAutopilot(string theory)
    {
        Logger.Event("Faking starts using " + theory);

        playerState.CmdSetFakingState(theory);

        // Start spawning tokens
        tokenSpawner.active = true;

        // Flash the screen
        flashScreen.Flash();

        // Turn off body trackers and controllers
        GetComponent<PlayerMovementControl> ().SetControl (false);

        // Block the player from talking
        GetComponent<PlayerTalking>().Block();

        // Turn on faking generators
        SetFakingGenerators(true);

        // Register for head nods generation
        GetComponent<PubSubClient>().CmdSubscribeToHeadNods(theory);

        // Instantiate the hidden player and control it
        hiddenPlayerObj = Instantiate (hiddenPlayerPrefab, transform.position, transform.rotation);
        hiddenPlayerObj.GetComponent<PlayerMovementControl> ().SetControl (true);

        // Hide me
        SetVisibility (false);
    }


    private void StopAutopilot()
    {
        Logger.Event("Faking stops");

        playerState.CmdSetFakingState(null);

        // Stop spawning tokens
        tokenSpawner.active = false;

        // Flash the screen
        flashScreen.Flash();

        // Unregister from head nods
        GetComponent<PubSubClient>().CmdUnsubscribeFromHeadNods();

        // Turn off faking generators
        SetFakingGenerators(false);

        // Jump back into the faking avatar
        if (SceneManager.GetActiveScene().name != "Simulator")
        {
            ResetCamera();
        }

        // Allow the player to talk again
        GetComponent<PlayerTalking>().Unblock();

        // Turn on body trackers
        GetComponent<PlayerMovementControl> ().SetControl (true);

        // Destroy the hidden player
        Destroy(hiddenPlayerObj);
        hiddenPlayerObj = null;

        // Show me
        SetVisibility (true);
    }


    private void SetVisibility(bool onOff) {
        foreach (var r in gameObject.GetComponentsInChildren<MeshRenderer> ()) {
            r.enabled = onOff;
        }
        foreach(var c in GetComponents<DetachedChild>())
        {
            c.SetVisibility(onOff);
        }
    }


    private void SetFakingGenerators(bool onOff)
    {
        GetComponent<LookAtSpeaker>().active = onOff;
        GetComponent<PlayerAccuses>().enabled = !onOff;

        foreach (var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.active = onOff;
        }
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
