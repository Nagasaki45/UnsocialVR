using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutomation : MonoBehaviour {

    public int minimumTimeBetweenAutomations;
    public float chanceToStartAutomation;
    public string[] automationModels;

    public string automationModel;
    public Transform trueHead;
    public Transform remoteHead;

    private float lastAutomationFinished = 0.0f;
    private PlayerNodding playerNodding;


    void Start()
    {
        playerNodding = GetComponent<PlayerNodding>();
    }


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
            float rand = UnityEngine.Random.Range(0, 1);
            if (Time.time - lastAutomationFinished > minimumTimeBetweenAutomations && rand < chanceToStartAutomation)
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
            int i = UnityEngine.Random.Range(0, automationModels.Length);
            automationModel = automationModels[i];
            Logger.Event("Partner automation started using " + automationModel);
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
        if (IsAutomated() && automationModel == model)
        {
            playerNodding.Nod();
        }
    }
}
