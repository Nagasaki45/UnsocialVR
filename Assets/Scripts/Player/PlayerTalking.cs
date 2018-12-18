using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Dissonance;
using Dissonance.VAD;


public class PlayerTalking : NetworkBehaviour {

    [SyncVar]
    public bool isTalking;

    public float talkingAmplitudeThreshold;

    private DissonanceComms comms;


    private void Start()
    {
        comms = GameObject.FindGameObjectWithTag ("DissonanceSetup").GetComponent<DissonanceComms> ();
    }


    private void Update ()
    {
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


    bool IsThereSpeechInAudio () {
        foreach (VoicePlayerState voicePlayerState in comms.Players)
        {
            // UglyHack (c) to check the type of a private class.
            if (voicePlayerState.GetType ().Name == "LocalVoicePlayerState")
            {
                return voicePlayerState.IsSpeaking && (voicePlayerState.Amplitude > talkingAmplitudeThreshold);
            }
        }
        return false;
    }
}
