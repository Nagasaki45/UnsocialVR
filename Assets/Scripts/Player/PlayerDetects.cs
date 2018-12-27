﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDetects : MonoBehaviour {

    public string controllerTag;
    public PlayerScore playerScore;
    public AudioClip correctAudioClip;
    public AudioClip incorrectAudioClip;

    SteamVR_TrackedObject trackedObj;
    PlayerPartner playerPartner;
    AudioSource audioSource;

    SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int) trackedObj.index); }
    }


    void Start()
    {
        trackedObj = GameObject.FindGameObjectWithTag(controllerTag).GetComponent<SteamVR_TrackedObject>();
        playerPartner = GetComponent<PlayerPartner>();
        audioSource = GetComponent<AudioSource>();
    }


    void Update() {
        if (Controller.GetHairTriggerDown())
        {
            DetectPartner();
        }
    }


    void DetectPartner()
    {
        bool correct = playerPartner.IsAutomated();

        playerScore.Add(correct ? 1 : -1);  // TODO more points for correct accusation
        audioSource.clip = correct? correctAudioClip : incorrectAudioClip;
        audioSource.Play();

        Logger.Event($"Detecting - Correct: {correct}");

        playerPartner.StopAutomation();
    }

}
