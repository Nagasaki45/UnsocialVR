using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour {

    [SyncVar(hook = "OnTalkingChange")]
    public bool isTalking;

    float talkingAmplitudeThreshold;
    DissonanceComms comms;


    private void Start()
    {
        comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();
    }


    private void Update ()
    {
        // Local players to update isTalking
        if (!isLocalPlayer)
        {
            return;
        }
        bool speechDetected = IsThereSpeechInAudio();
        if (speechDetected && !isTalking)
        {
            CmdSetTalkingState(true);
        }
        else if (!speechDetected && isTalking)
        {
            CmdSetTalkingState(false);
        }
    }


    [Command]
    void CmdSetTalkingState (bool onOff)
    {
        isTalking = onOff;
    }


    void OnTalkingChange(bool newState)
    {
        isTalking = newState;
        if (isLocalPlayer)
        {
            Logger.Event($"Talking - State: {newState}");
        }
    }


    bool IsThereSpeechInAudio () {
        foreach (VoicePlayerState voicePlayerState in comms.Players)
        {
            // UglyHack (c) to check the type of a private class.
            if (voicePlayerState.GetType ().Name == "LocalVoicePlayerState")
            {
                return voicePlayerState.IsSpeaking && (voicePlayerState.Amplitude > talkingAmplitudeThreshold);
            }
        }
        Debug.LogError("Failed to detect talking state");
        return false;
    }
}
