﻿using UnityEngine;
using System.Collections;

public class KeyManager : MonoBehaviour {

    //Public variables
    public string[] keys;

    //Objects
    private GameObject player;
    private GameObject playerInteract;
    private GameObject sprite_inventory;
    private GameObject obj_camera;

    //Scripts
    private PlayerController plController;
    private Interaction plInteraction;
    private InventoryUI inventoryUI;
    private CameraControl camMov;

    void Start () {
        //Dependency "Player"
        player = GameObject.FindGameObjectWithTag("Player");
        playerInteract = GameObject.FindGameObjectWithTag("PLAYER_interact");
        plController = player.GetComponent<PlayerController>();
        plInteraction = playerInteract.GetComponent<Interaction>();
        //Dependency "Inventory"
        sprite_inventory = GameObject.FindGameObjectWithTag("GLOBAL_inventoryUI");
        inventoryUI = sprite_inventory.GetComponent<InventoryUI>();

        obj_camera = GameObject.FindGameObjectWithTag("GLOBAL_camera");
        camMov = obj_camera.GetComponent<CameraControl>();
	}
	
	void Update () {
        //Player movement
        if (Input.GetKey(keys[0]))
        {
            plController.MoveRight();
        }
        else if (Input.GetKey(keys[1]))
        {
            plController.MoveLeft();
        }
        else
        {
            plController.Idle();
        }
        if (Input.GetKey(keys[2]))
        {
            plController.MoveUp();
        }
        else if (Input.GetKey(keys[3]))
        {
            plController.MoveDown();
        }
        //Pick up item or interact with object
        if (Input.GetKeyDown(keys[4]))
        {
            plInteraction.PickUp();
            plInteraction.Talk();
        }
        //Change inventory state
        if (Input.GetKeyDown(keys[5]))
        {
            inventoryUI.ChangeState();
        }
        //Screech
        if (Input.GetKey(keys[6]))
        {
            plInteraction.Screech(true);
        }
        else if(Input.GetKeyUp(keys[6]))
        {
            plInteraction.ScreechStop(true);
        }
        if (Input.GetKey(keys[7]))
        {
            plInteraction.Screech(false);
        }
        else if (Input.GetKeyUp(keys[7]))
        {
            plInteraction.ScreechTimerStop();
        }
        else if(!Input.GetKey(keys[6]) && !Input.GetKeyUp(keys[7]))
        {
            plInteraction.ScreechStop(false);
        }
    }
}
