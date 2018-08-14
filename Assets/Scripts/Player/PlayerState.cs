using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : NetworkBehaviour {

    public int score;

    [SyncVar]
    string fakingTheory;


    void Start()
    {
        fakingTheory = null;
        score = 0;
    }


    [Command]
    public void CmdSetFakingState(string newFakingTheory)
    {
        fakingTheory = newFakingTheory;
    }


    public bool IsFaking()
    {
        return !String.IsNullOrEmpty(fakingTheory);
    }


    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GUI.contentColor = Color.black;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 200));
        GUILayout.Label("Score: " + score);
        if (IsFaking())
        {
            GUILayout.Label("Faking using " + fakingTheory);
        }
        GUI.EndGroup();
    }
}
