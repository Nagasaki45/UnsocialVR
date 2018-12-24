using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutomation : MonoBehaviour {

    public int minimumTimeBetweenAutomations;
    public string[] automationModels;
    public string[] animations;
    public float animationCrossfadeSeconds;

    public string automationModel;
    public Transform trueHead;
    public Transform remoteHead;
    public Animator animator;

    private float lastAutomationFinished = 0.0f;


    void Update()
    {
        if (!IsAutomated())
        {
            remoteHead.position = trueHead.position;
            remoteHead.rotation = trueHead.rotation;
        }
        else
        {
            // TODO
        }
    }


    public bool IsAutomated()
    {
        return !String.IsNullOrEmpty(automationModel);
    }


    public void ToggleAutomation()
    {
        if (!IsAutomated())
        {
            if (Time.time - lastAutomationFinished > minimumTimeBetweenAutomations)
            {
                StartAutomation();
            }
        }
        else
        {
            StopAutomation();
        }
    }


    public void StartAutomation()
    {
        if (!IsAutomated())
        {
            bool speaker = GetComponentInParent<PlayerTalking>().speaker;
            int i = UnityEngine.Random.Range(0, automationModels.Length);
            automationModel = automationModels[i];
            Logger.Event($"Partner automation started - Model: {automationModel}. Speaker: {speaker}");
        }
    }


    public void StopAutomation()
    {
        if (IsAutomated())
        {
            automationModel = null;
            lastAutomationFinished = Time.time;
            Logger.Event("Partner automation stopped");
        }
    }


    public void Nod(string model)
    {
        if (automationModel != model)
        {
            return;
        }
        bool speaker = GetComponentInParent<PlayerTalking>().speaker;
        int i = UnityEngine.Random.Range(0, animations.Length);
        string animation = animations[i];
        Logger.Event($"Partner automatic nod - Model: {automationModel}. Speaker: {speaker}. Animation: {animation}");
        animator.CrossFade(animation, animationCrossfadeSeconds);
    }
}
