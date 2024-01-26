using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Movement variables
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    //Combat variables
    [SerializeField] public float damage = 20f;
    [SerializeField] private bool isGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private CharacterController controller;

    //Animation variables
    private Animator animator;
    private float timerForNextAttack = 3f;
    private bool startCoolDown = false;
    private bool isMoveable = true;
    public bool isDefend = false;

    //Health and UI variables
    [SerializeField] private float MaxHealth;
    private float health;
    public Image greenBar;
    public Image attackIcon;

    //Post processing variable
    public GameObject pp;// ref to post processing object

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = MaxHealth;// Set health to level default health
    }

    private void Update()
    {
        //Functions that need to be called each frame
        CheckHealth();
        CoolDown();
        Move();
        DisplayHealth();
        AttackIconDisplay();
    }

    private void Move()
    {
        //Ground check, player can only move when it on the ground
        isGround = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        //Moveable check, when play attack animation player can not move
        if (isMoveable)
        {
            if (isGround && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //Get input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection = transform.TransformDirection(moveDirection);

            if (isGround)
            {
                if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
                {
                    //walk
                    Walk();
                }
                else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
                {
                    //Run
                    Run();
                }
                else if (moveDirection == Vector3.zero)
                {
                    //Idle
                    Idle();
                }
                moveDirection *= moveSpeed;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Jump
                    Jump();
                }
            }

            //Apply movement and gravity
            controller.Move(moveDirection * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        //Attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && timerForNextAttack >= 3f)
        {
            Attack();
        }
        //Defend when hold mouse right key
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Defend(); // Defend animation has no exit time and will play repeatly when right key was holden
        }
        //Relase mouse right key will set a animator trigger that exit defend animation
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetTrigger("Release");// exit defend animation
            isDefend = false; // Player will be able to play other animation 
        }

    }

    //Blend tree with three motion. 0 as idle, 0.5 as walk, 1 as run. add 0.1f for smooth motion transfer
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        moveSpeed = walkSpeed;
    }

    private void Run()
    {
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        moveSpeed = runSpeed;
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    // Attack function will set player not moveable and trigger attack animation. Attack will have
    // 3 seconds cooldown time. After each attack, the cooldown timer will start count up. Next attack
    //will be available when timer exceed 3 seconds.
    private void Attack()
    {
        isMoveable = false;
        animator.SetTrigger("Attack");
        startCoolDown = true;
        timerForNextAttack = 0;
    }

    private void Defend()
    {
        animator.SetTrigger("Defend");
        isDefend = true;
    }

    //AttackFinish funciton will be called when animation finished
    public void AttackFinish()
    {
        isMoveable = true;
    }

    //This is the cooldown function it will check if player used attack function, if used, start count up.
    //When timer exceed 3 seconds, player are able to perform another attack and timer reset.
    private void CoolDown()
    {
        if (startCoolDown)
        {
            timerForNextAttack += Time.deltaTime;
        }
        else if (timerForNextAttack >= 3f)
        {
            startCoolDown = false;
        }
    }

    //Let enemy call this function to apply damage to the player and play GetHit animation
    public void TakeDamage(float amount)
    {
        isMoveable = false;
        health -= amount;
        animator.SetTrigger("GetHit");
    }

    //When player collide with Heal object, heal player with a default amount
    public void Heal(float amount)
    {
        if ((health + amount) > MaxHealth)
        {
            health = MaxHealth;
        }
        else
        {
            health += amount;
        }

    }

    //Check player health each frame and if health below 50, it will enable a post processing to indicate
    //player is in low health. If health reach 0, player dead.
    private void CheckHealth()
    {
        if(health > 50)
        {
            pp.GetComponent<PostProcessVolume>().enabled = false;
        }
        if(health > 0 && health <= 40)
        {
            pp.GetComponent<PostProcessVolume>().enabled = true;
        }

        if (health <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    //Control the player health bar
    private void DisplayHealth()
    {
        greenBar.fillAmount = health / MaxHealth;
    }

    //Control the attack icon 
    private void AttackIconDisplay()
    {
        attackIcon.fillAmount = timerForNextAttack / 3;
    }

    //Play dead animation and load Lose scene after 2 seconds
    private IEnumerator Dead()
    {
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Lose");
    }
}
