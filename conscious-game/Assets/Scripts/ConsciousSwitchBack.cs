using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsciousSwitchBack : MonoBehaviour
{
    public Behaviour[] scripts;
    public GameObject GFX;

    public void OnPlayerSwitch()
    {
        GFX.layer = 12; //changing layers to the can see world model
    }

    public void OnDisableScripts()
    {
        for (int i = 0; i < scripts.Length; i++)
        {

            scripts[i].enabled = false;


        }

    }

    public void OnSwitchBack()
    {

        
        for (int i = 0; i < scripts.Length; i++)
        {

            scripts[i].enabled = true;


        }

    }
}
