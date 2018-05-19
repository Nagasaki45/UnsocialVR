using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAccuses : MonoBehaviour {

    public string controllerTag;

    private PlayerLogger logger;
    private PlayerGaze playerGaze;
    private PlayerScore playerScore;
    private AudioSource audioSource;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    private void Awake()
    {
        logger = GetComponent<PlayerLogger>();
        audioSource = GetComponent<AudioSource> ();
        playerGaze = GetComponent<PlayerGaze> ();
        playerScore = GetComponent<PlayerScore>();
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene ().name != "Simulator")
        {
            trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
        }
    }


    private void Update() {
        if (SceneManager.GetActiveScene ().name == "Simulator")
        {
            if (Input.GetButtonDown ("Accuse")) {
                AccusePlayer (playerGaze.gazedObj);
            }
        }
        else
        {
            if (Controller.GetHairTriggerDown ())
            {
                AccusePlayer (playerGaze.gazedObj);
            }
        }
    }


    private void AccusePlayer(GameObject accusedPlayer)
    {
        if (null != accusedPlayer)
        {
            bool correct = accusedPlayer.GetComponent<PlayerAutopilot>().isFaking;
            int score = correct ? 1 : -1;
            string text = correct ? "Correctly" : "Mistakenly";
            logger.Event(text + " accusing " + playerGaze.GetGazedNetId() + " for faking");
            playerScore.CmdAdd(score);
        }

        audioSource.Play ();
    }

}
