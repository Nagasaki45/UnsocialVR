using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScore : NetworkBehaviour {

    [SyncVar]
    int score;

    void Start()
    {
        score = 0;
    }


    public void CmdAdd(int value)
    {
        score += value;
    }


    public void OnGUI()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height - 50, 100, 100));
        GUILayout.Label("Score: " + score);
        GUI.EndGroup();
    }
}
