using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartner : MonoBehaviour {
    // An interface for PlayerAutomation, which is a script on Remote,
    // from Local part.

    public GameObject GetPartner()
    {
        GameObject me = transform.parent.gameObject;  // Player/Local
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) {
            if (p != me)
            {
                return p;
            }
        }
        return null;
    }


    public bool IsAutomated()
    {
        GameObject partner = GetPartner();
        if (partner != null)
        {
            return partner.GetComponentInChildren<PlayerAutomation>().IsAutomated();
        }
        return false;
    }


    public void StopAutomation()
    {
        GameObject partner = GetPartner();
        if (partner != null)
        {
            partner.GetComponentInChildren<PlayerAutomation>().StopAutomation();
        }
    }


    public void Nod(string model)
    {
        Logger.Event($"Player initiates nod in partner - Model: {model}");
        GameObject partner = GetPartner();
        if (partner != null)
        {
            partner.GetComponentInChildren<PlayerAutomation>().Nod(model);
        }
    }
}
