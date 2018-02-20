using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovementControl : MonoBehaviour {


    public void SetControl(bool state)
    {
        if (SceneManager.GetActiveScene ().name == "Simulator") {
            GetComponent<PlayerSimulatorControl> ().enabled = state;
        }
        else
        {
            foreach (var binding in GetComponents<BindTransform> ())
            {
                binding.enabled = state;
            }
        }
    }
}
