using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerAutopilot : MonoBehaviour {

    public string controllerTag;
    public string[] fakingTheories;
    public GameObject snakeGamePrefab;
    public Transform snakeGameParent;

    private GameObject snakeGame;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    void Start()
    {
        trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
    }


    void Update()
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


    void StartAutopilot(string theory)
    {
        Logger.Event("Faking starts using " + theory);

        GetComponentInParent<PlayerState>().CmdSetFakingState(theory);

        // Block the player from talking
        GetComponentInParent<PlayerTalking>().Block();

        // Turn on faking generators
        SetFakingGenerators(true);

        // Register for head nods generation
        GetComponentInParent<PubSubClient>().CmdSubscribeToHeadNods(theory);

        // Start the snake game in front of head
        snakeGame = Instantiate(snakeGamePrefab, snakeGameParent);
        snakeGame.transform.parent = snakeGameParent;
    }


    private void StopAutopilot()
    {
        Logger.Event("Faking stops");

        GetComponentInParent<PlayerState>().CmdSetFakingState(null);

        // Allow the player to talk again
        GetComponentInParent<PlayerTalking>().Unblock();

        // Turn off faking generators
        SetFakingGenerators(false);

        // Unregister from head nods
        GetComponentInParent<PubSubClient>().CmdUnsubscribeFromHeadNods();

        Destroy(snakeGame);

    }


    private void SetFakingGenerators(bool onOff)
    {
        GetComponent<PlayerAccuses>().enabled = !onOff;

        GameObject performative = transform.parent.Find("Performative").gameObject;
        performative.GetComponentInChildren<LookAtSpeaker>().active = onOff;

        foreach (var naturalMovement in performative.GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.active = onOff;
        }
    }
}
