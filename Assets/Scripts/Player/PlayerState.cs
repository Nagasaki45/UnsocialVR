using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : NetworkBehaviour {

    public int score;
    public TextMesh scoreTextMesh;

    [SyncVar]
    string fakingTheory;

    [SyncVar]
    int chair;


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
    public void CmdSetChair(int x)
    {
        chair = x;
    }

    public int GetChair()
    {
        return chair;
    }


    public bool IsFaking()
    {
        return !String.IsNullOrEmpty(fakingTheory);
    }


    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        scoreTextMesh.text = "Score: " + score;
    }


    void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GUI.contentColor = Color.black;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 200));
        if (IsFaking())
        {
            GUILayout.Label("Faking using " + fakingTheory);
        }
        GUI.EndGroup();
    }
}
