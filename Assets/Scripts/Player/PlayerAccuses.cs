using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAccuses : MonoBehaviour {

    public string controllerTag;

    private PlayerGaze playerGaze;
    private PlayerState playerState;
    private AudioSource audioSource;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    private void Awake()
    {
        audioSource = GetComponent<AudioSource> ();
        playerGaze = GetComponent<PlayerGaze> ();
        playerState = GetComponent<PlayerState>();
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
            bool correct = accusedPlayer.GetComponent<PlayerState>().IsFaking();
            int score = correct ? 1 : -1;
            string text = correct ? "Correctly" : "Mistakenly";
            Logger.Event(text + " accusing " + playerGaze.GetGazedNetId() + " for faking");
            playerState.CmdAddScore(score);
        }

        audioSource.Play ();
    }

}
