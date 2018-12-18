using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartner : MonoBehaviour {


    private GameObject GetPartner(GameObject me)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) {
            if (p != me)
            {
                return p;
            }
        }
        return null;
    }


    public bool IsFaking()
    {
        // TODO
        return true;
    }


    public void StopFaking()
    {
        // TODO
    }


    public void Nod(string theory)
    {
        // TODO
    }
}
