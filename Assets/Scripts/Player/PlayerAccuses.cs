using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAccuses : MonoBehaviour {

    public string controllerTag;
    public PlayerGaze playerGaze;
    public PlayerState playerState;
    public AudioSource audioSource;
    public AudioClip correctAudioClip;
    public AudioClip incorrectAudioClip;

    SteamVR_TrackedObject trackedObj;

    SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    void Start()
    {
        trackedObj = GameObject.FindGameObjectWithTag(controllerTag).GetComponent<SteamVR_TrackedObject>();
    }


    void Update() {
        if (Controller.GetHairTriggerDown() && playerGaze.gazedObj != null)
        {
            AccusePlayer(playerGaze.gazedObj);
        }
    }


    void AccusePlayer(GameObject accusedPlayer)
    {
        bool correct = accusedPlayer.GetComponent<PlayerState>().IsFaking();

        playerState.score += correct ? 1 : -1;

        audioSource.clip = correct? correctAudioClip : incorrectAudioClip;
        audioSource.Play();

        string text = correct ? "Correctly" : "Mistakenly";
        Logger.Event(text + " accusing " + playerGaze.GetGazedNetId() + " for faking");
    }

}
