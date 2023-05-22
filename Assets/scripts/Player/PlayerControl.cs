using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private bool isFaceRight = true;
    private float moveDirection;

    private Rigidbody2D rb;
    private Animator anim;
    private GameObject playerStates;
    

    private float isRun = 0.0f;
    private int isRunHash;

    public float MoveSpeed = 3.0f;
    public float jumpForce = 2.0f;

    //ground
    public Transform groundCheck;
    public bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    //jump
    public bool canJump;
    public int amountOfJumps = 1;
    private int amountOfJumpsLeft;
    //attack
    public bool isAttack;
    public GameObject attackColi;
   
    //hurt
    public bool isHurt;

    //timer
    private bool attackLock = false;
    private float tempCoolDown;
    public float coolDown;



    
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStates = GameObject.Find("PlayerHp");
        isRunHash = Animator.StringToHash("isRun");
        amountOfJumpsLeft = amountOfJumps;
        tempCoolDown = coolDown;
        
    }
    void Update()
    {
        CheckInput(); 
        CheckFace();
        UpdateAnim();
        CheckIfCanJump();
        if (attackLock == true)
        {
            AttackTimer();
        }




    }


    private void FixedUpdate()
    {
        ApplyMove();
        CheckSurroundins();
        Attack();

    }

    
    private void CheckInput()
    { 
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        { 
            Jump();   
        }
    }
    
    private void UpdateAnim()
    {
        if (Input.GetButton("Horizontal"))
        {
            isRun = 1;
        }
        else
        {
            isRun = 0;
        }

        if (isHurt == true && anim.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            isHurt = false;
        }



        anim.SetFloat(isRunHash, isRun);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isHurt", isHurt);
    }
    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
           amountOfJumpsLeft = amountOfJumps ;
        }
        if(amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
   
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            amountOfJumpsLeft--;
        } 
    }

    private void ApplyMove()
    {
        rb.velocity = new Vector2(MoveSpeed * moveDirection, rb.velocity.y);
    }
    
    
    private void CheckFace()
    {
        if(isFaceRight && moveDirection < 0)
        {
            Flip();
        }
        else if(!isFaceRight && moveDirection > 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        isFaceRight = !isFaceRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void CheckSurroundins()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }

    private void AttackTimer()
    {
        
        tempCoolDown -= Time.deltaTime;

        if (tempCoolDown <= 0)
        {
            attackLock = false;
            tempCoolDown = coolDown;
        }
        else
        {
            attackLock = true;
        }
    }
        
    private void Attack()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !attackLock)
        {
            
            isAttack = true;
            attackColi.SetActive(true);
            Debug.Log("Hit!!");
            attackLock = true;

        }
        else
        {
            isAttack = false;
            attackColi.SetActive(false);

        }

    }
    
    //coil hit tag
    private  void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag=="Enemy" ) //attack by enemy
        {
            isHurt = true;
            playerStates.GetComponent<PlayerStates>().player_hp-=1;

         
        }
        
      }
    

}
