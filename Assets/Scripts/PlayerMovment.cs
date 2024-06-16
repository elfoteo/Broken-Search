using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public Animator animator;
    public PlayerController controller;
    public float runSpeed = 40.0f;
    float horizontalMove = 0.0f;
    bool jump = false;
    public Player player;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
    }

    public void OnLanding ()
    {
        animator.SetBool("isJumping", false);
    }


    private void FixedUpdate()  
    {
        if (player.isDead)
        {
            return; // Blocca il movimento del giocatore se è morto
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
