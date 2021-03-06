﻿using UnityEngine;
using System.Collections;

public class OF_Extinguisher : MonoBehaviour {

    GameObject player;
    ObjectVariables var;
    AudioSource aud;
    PlayerVariables var_player;
	// Use this for initialization
	void Start () {
        var = GetComponent<ObjectVariables>();
        aud = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        var_player = player.GetComponent<PlayerVariables>();
	}
	
	// Update is called once per frame
	void Update () {
        if (var.used)
        {
            if (!aud.isPlaying && var_player.im_event == false)
            {
                aud.Play();
                var_player.im_event = true;
            }
            if (!aud.isPlaying)
            {
                var_player.im_event = false;
                var.used = false;
            }
        }
	}
}
