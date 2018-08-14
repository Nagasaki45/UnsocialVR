using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : NetworkBehaviour {

    [SyncVar]
    string fakingTheory;

    [SyncVar]
    int score;


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


    [Command]
    public void CmdAddScore(int value)
    {
        score += value;
    }


    public bool IsFaking()
    {
        return !String.IsNullOrEmpty(fakingTheory);
    }


    public int GetScore()
    {
        return score;
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
