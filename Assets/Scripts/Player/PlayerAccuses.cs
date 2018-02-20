using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerAccuses : NetworkBehaviour {

    public string controllerTag;
    public AudioClip accusingNothing;
    public AudioClip accusingPlayer;

    private PlayerGaze playerGaze;
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
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene ().name != "Simulator")
        {
            trackedObj = GameObject.FindGameObjectWithTag (controllerTag).GetComponent<SteamVR_TrackedObject> ();
        }
    }


    private void Update() {
        if (!isLocalPlayer)
        {
            return;
        }

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
        if (null == accusedPlayer)
        {
            audioSource.clip = accusingNothing;
        }
        else
        {
            audioSource.clip = accusingPlayer;

            // TODO do something about it
        }

        audioSource.Play ();
    }

}
