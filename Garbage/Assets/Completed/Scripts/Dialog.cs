﻿using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {

    public bool activated = true;
    [Header("GameObjects")]
    public GameObject dialogObj;
    GameObject player;
    [Header("Scripts")]
    public NPCBehavior movement;
    [Space(10)]
    [Range(0, 10)]
    public int range;
    public Sprite portrait;
    [TextArea(3, 10)]
    public string[] lines;
    public GameObject[] items;
    public GameObject[] events;

    DialogFunction dialog;
    float leftRange;
    float rightRange;
    bool currentlyTalking;

    GridVariables NPCGridVar;
    GridVariables plGridVar;

    void Start () {
        dialog = dialogObj.GetComponent<DialogFunction>();
        player = GameObject.FindGameObjectWithTag("Player");
        plGridVar = player.GetComponent<GridVariables>();
        NPCGridVar = GetComponent<GridVariables>();
	}
	
	// Update is called once per frame
	void Update () {
        leftRange = transform.position.x - range;
        rightRange = transform.position.x + range;
	    if (Input.GetKeyDown("d") && activated && player.transform.position.x < rightRange && player.transform.position.x > leftRange && NPCGridVar.gridLayer == plGridVar.gridLayer)
        {
            dialog.startDialog(lines, items, events, portrait);
            currentlyTalking = true;
            activated = false;
        }
        if (currentlyTalking && dialog.active == false)
        {
            currentlyTalking = false;
        }
        if (currentlyTalking)
        {
            movement.active = false;
        }
        else
        {
            movement.active = true;
        }
	}
}