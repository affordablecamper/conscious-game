using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsciousSwitch : MonoBehaviour
{
    
    public Behaviour[] scriptsToEnable;
    private ConsciousSwitchBack backToPlayer;
    public bool startTimer;
    [SerializeField]
    private float timer;
    public float switchTime = 10f;

    private void Start()
    {
        backToPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ConsciousSwitchBack>();
        for (int i = 0; i < scriptsToEnable.Length; i++)
        {

            scriptsToEnable[i].enabled = false;


        }
    }

    public void OnSwitch () 
    {
        //backToPlayer.OnDisableScripts();
        for (int i = 0; i < scriptsToEnable.Length; i++)
        {

            scriptsToEnable[i].enabled = true;


        }
        timer = switchTime;
        startTimer = true;
        backToPlayer.OnDisableScripts();
    }

    private void Update()
    {
        if (startTimer)
        {

            if (timer <= 0)
            {

                OnSwitchBack();

            }

        }

        timer -= Time.deltaTime;
    }

    private void OnSwitchBack()
    {

        
        backToPlayer.OnSwitchBack();
        Destroy(gameObject);
    }
}
