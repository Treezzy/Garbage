﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject playerSprite;
    public GameObject gridObject;
    JensAnimationController playerAnimation;
    SpriteSize playerSize;
	private PlayerVariables playerVar;
	private Animator animator;
	private Rigidbody2D rb;
    GridVariables gridVar;

    public bool movingUp;
    public bool movingDown;

	void Start ()
	{
		animator = playerSprite.GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		playerVar = GetComponent<PlayerVariables> ();
        gridVar = GetComponent<GridVariables>();
        playerAnimation = playerSprite.GetComponent<JensAnimationController>();
        playerSize = playerSprite.GetComponent<SpriteSize>();
	}
	
	void Update ()
	{
        if (Input.GetKey("left"))
		{
            //rb.MovePosition(transform.position - transform.right * playerVar.speed * Time.deltaTime);
            transform.position += new Vector3(-playerVar.speed * Time.deltaTime, 0, 0);
            if (gridVar.canMove == true)
            {
                animator.SetInteger("Direction", 1);
            }
		}

        if (Input.GetKey("right"))
        {
            //rb.MovePosition(transform.position + transform.right * playerVar.speed * Time.deltaTime);
            transform.position += new Vector3(playerVar.speed * Time.deltaTime, 0, 0);
            if (gridVar.canMove == true)
            {
                animator.SetInteger("Direction", 3);
            }
        }
        if (Input.GetKey("up") && gridVar.canMove)
        {
            gridVar.gridLayer++;
            gridVar.canMove = false;
            animator.SetInteger("Direction", 2);
        }
        if (Input.GetKey("down") && gridVar.canMove)
        {
            gridVar.gridLayer--;
            gridVar.canMove = false;
            animator.SetInteger("Direction", 0);
        }
    }
}