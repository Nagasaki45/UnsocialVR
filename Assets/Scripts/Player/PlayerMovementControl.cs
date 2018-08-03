using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovementControl : MonoBehaviour {


    public void SetControl(bool state)
    {
        foreach (var binding in GetComponents<BindTransform>())
        {
            binding.enabled = state;
        }
    }
}
