using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {

    int score;

    void Start()
    {
        score = 0;
    }


    public void Add(int value)
    {
        score += value;
    }


    public void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height - 50, 100, 100));
        GUILayout.Label("Score: " + score);
        GUI.EndGroup();
    }
}
