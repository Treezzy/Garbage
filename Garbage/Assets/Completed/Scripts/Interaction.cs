﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction : MonoBehaviour {

    //Public variables
    public GameObject textBox;
    public GameObject itemUI;
    [Space(5)]
    public int distance;
    [Header("Read-only")]
    public bool canPickup;

    //Objects
    private GameObject object_Inventory;
    private GameObject player;
    private GameObject cameraObject;
    public float timer;
    public float timer2;
    public GameObject bar;

    //Lists
    public AudioClip[] scrtype;
    public List<GameObject> list_pickup;  //List of type 0
    public List<GameObject> list_screech; //List of type 1
    public List<GameObject> list_event;   //List of type 2

    private List<GameObject> list_temp; //Temporary list of type 0
    private List<ObjectVariables> list_var1; //Variables of type 1


    //Scripts
    private Inventory inventory;
    private SpawnByLoudness micInput;
    private CameraControl camMov;
    private PlayerController plController;

    //Variables
    public bool screeching = false;

    void Start()
    {
        //Dependency "Inventory"
        object_Inventory = GameObject.FindGameObjectWithTag("GLOBAL_inventory");
        inventory = object_Inventory.GetComponent<Inventory>();
        //Dependency "Player"
        player = GameObject.FindGameObjectWithTag("Player");

        list_pickup = new List<GameObject>();
        list_screech = new List<GameObject>();
        list_event = new List<GameObject>();

        list_temp = new List<GameObject>();
        list_var1 = new List<ObjectVariables>();

        micInput = player.GetComponent<SpawnByLoudness>();

        cameraObject = GameObject.FindGameObjectWithTag("GLOBAL_camera");
        camMov = cameraObject.GetComponent<CameraControl>();

        plController = transform.parent.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (screeching)
        {
            for (int i = 0; i < list_screech.Count; i++)
            {
                list_var1[i].force += Time.deltaTime;
            }
        }
    }

    //Changes value of bool based on collision
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "INTERACT_pickup")
        {
            list_pickup.Add(col.gameObject);
            list_temp.Add(col.gameObject);
        }
        if (col.tag == "INTERACT_screech")
        {
            list_screech.Add(col.gameObject);
        }
        if (col.tag == "INTERACT_eventOnKey")
        {
            list_event.Add(col.gameObject);
        }
        if (col.tag == "INTERACT_event")
        {
            col.gameObject.GetComponent<ObjectVariables>().used = true;
        }
        if (col.tag == "INTERACT_pickup" || col.tag == "INTERACT_screech" || col.tag == "INTERACT_event")
        {
            ObjectVariables var = col.gameObject.GetComponent<ObjectVariables>();
            if (var.canHighlight)
            {
                var.highlight.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "INTERACT_pickup")
        {
            list_pickup.Remove(col.gameObject);
            list_temp.Remove(col.gameObject);
        }
        if (col.tag == "INTERACT_screech")
        {
            list_screech.Remove(col.gameObject);
        }
        if (col.tag == "INTERACT_eventOnKey")
        {
            list_event.Remove(col.gameObject);
        }
        if (col.tag == "INTERACT_pickup" || col.tag == "INTERACT_screech" || col.tag == "INTERACT_event")
        {
            ObjectVariables var = col.gameObject.GetComponent<ObjectVariables>();
            if (var.canHighlight)
            {
                var.highlight.SetActive(false);
            }
        }
    }
    public void PickUp()
    {
        //Check which item is closest to the player
        /*while (1 < list_pickup.Count) {
            Debug.Log("searching...");
            for (int i = 0; i < list_pickup.Count; i++)
            {
                for (int j = 0; j < list_pickup.Count;)
                {
                    if (j != i)
                    {
                        if (Vector2.Distance(player.transform.position, list_pickup[j].transform.position) > Vector2.Distance(player.transform.position, list_pickup[i].transform.position))
                        {
                            list_pickup.Remove(list_pickup[j]);
                            Debug.Log("Removed object j" + j);
                            j++;
                        }
                        else
                        {
                            list_pickup.Remove(list_pickup[i]);
                            Debug.Log("Removed object i" + i);
                            j = 1000000;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }*/
        list_pickup = checkList(list_pickup);
        if (list_pickup.Count > 0)
        {
            //Add item to inventory and destroy object
            var obj = (GameObject)Instantiate(list_pickup[0].GetComponent<ObjectVariables>().inventoryItem, transform.position, Quaternion.identity);
            inventory.AddItem(obj);
            GameObject singleObject = list_pickup[0];
            list_temp.Remove(singleObject);
            Destroy(singleObject);
            list_pickup = list_temp;
        }
    }
    public void Screech(bool mic)
    {
        if (mic)
        {
            if (micInput.ReadThreshold1())
            {
               
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[1]);
                    camMov.ScreenShake(true);
                }
                screeching = true;
            }
            else if (micInput.ReadThreshold2())
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[2]);
                    camMov.ScreenShake(true);
                }
                screeching = true;
            }
            else if (micInput.ReadThreshold3())
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[2]);
                    camMov.ScreenShake(true);
                }
                screeching = true;
            }
            else
            {
                if (screeching)
                {
                    list_var1.Clear();
                    plController.ScreechEnd();
                    camMov.StopShake(true);
                }
                screeching = false;
            }
        }
        else if (!mic)
        {
            bar.SetActive(true);
            bar.transform.localScale = new Vector3(1, timer, 1);
            timer += Time.deltaTime;
            if (timer >= 3.75)
            {
                timer = 3.75f;
            }

        }
    }
    public void ScreechTimerStop()
    {

        timer2 = (int)(timer * (1/0.75f))*0.75f;
        if(timer2 >= 3.75)
        {
            timer2 = 3.75f;
        }
        if(timer2 == 0)
        {
            timer = -1;
        }
    }
    public void ScreechStop(bool mic)
    {
        if (!mic && timer2 > 0)
        {
            timer2 -= Time.deltaTime;
            bar.SetActive(true);
            bar.transform.localScale = new Vector3(1, timer2, 1);
            
            if (timer >= 0.75 && timer < 1.5)
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    Debug.Log(54);
                    plController.Screech(scrtype[1]);
                    camMov.ScreenShake(true);
                    timer = 0;
                }
                screeching = true;
            }
            else if (timer >= 1.5 && timer < 2.25)
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[2]);
                    camMov.ScreenShake(true);
                    timer = 0;
                }
                screeching = true;
            }
            else if (timer >= 2.25 && timer < 3)
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[3]);
                    camMov.ScreenShake(true);
                    timer = 0;
                }
                screeching = true;
            }
            else if (timer >= 3 && timer < 3.75)
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[4]);
                    camMov.ScreenShake(true);
                    timer = 0;
                }
                screeching = true;
            }
            else if (timer >= 3.75)
            {
                if (!screeching)
                {
                    for (int i = 0; i < list_screech.Count; i++)
                    {
                        list_var1.Add(list_screech[i].GetComponent<ObjectVariables>());
                    }
                    plController.Screech(scrtype[5]);
                    camMov.ScreenShake(true);
                    timer = 0;
                }
                screeching = true;
            }
        }
        else if(!mic && timer2 <= 0)
        {
            if(timer == -1)
            {
                plController.aud.clip = scrtype[0];
                plController.aud.Play();
                timer = 0;
            }
            if (screeching)
            {
                list_var1.Clear();
                plController.ScreechEnd();
                camMov.StopShake(true);
                bar.SetActive(false);
                timer = 0;
            }
            screeching = false;
        }
        else if(mic)
        {
            if (screeching)
            {
                list_var1.Clear();
                plController.ScreechEnd();
                camMov.StopShake(true);
            }
            screeching = false;
        }
    }
    public void Talk()
    {
        if (list_event.Count > 0)
        {
            GameObject nearPlayer = checkList(list_event)[0];
            ObjectVariables vars = nearPlayer.GetComponent<ObjectVariables>();
            if (inventory.ItemExists(vars.itemID) || vars.requiresItem == false)
            {
                if (!vars.toggle)
                {
                    vars.used = true;
                }
                else
                {
                    vars.used = !vars.used;
                }
                /*else
                {
                    vars.used = false;
                }*/
            }
        }
    }
    public List<GameObject> checkList(List<GameObject> list)
    {
        //Check which object is closest to the player
        while (1 < list.Count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count;)
                {
                    if (j != i)
                    {
                        if (Vector2.Distance(player.transform.position, list[j].transform.position) > Vector2.Distance(player.transform.position, list[i].transform.position))
                        {
                            list.Remove(list[j]);
                            j++;
                        }
                        else
                        {
                            list.Remove(list[i]);
                            j = 1000000;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }
            }
        }
        return list;
    }
}
