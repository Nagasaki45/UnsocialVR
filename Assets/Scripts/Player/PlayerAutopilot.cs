using System.Collections;
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
        foreach (var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.active = onOff;
        }
        GetComponent<Notifier>().active = onOff;
        // TODO more faking generators
    }
}
