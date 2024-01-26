using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManagement : MonoBehaviour
{
    private NavMeshAgent Enemy;
    public Transform Player;
    public Transform EndPoint;
    private float distance;
    private bool isNearby = false;
    private bool isAttack = false;

    //Enemy movement and detect variables
    [SerializeField] public float damage = 20f;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float detectDistance = 15f;
    [SerializeField] private float attackDistance = 4f;
    private Animator animator;

    //Enemy has a attack cooldown time
    private float coolDown = 1.5f, timerForNextAttack=0f;
    private bool startCoolDown = false;

    //Enemy has a default health of 100
    private float health = 100f;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Enemy.speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CoolDown();
        CheckDistacne();
        Move();
        CheckHealth();
    }

    private void Move()
    {
        // Move funciton will first check if player is near by, if true enemy will check if it is in attack range
        // if true will attack, if false will run to player
        if (isNearby)
        {
            Enemy.transform.LookAt(Player);
            if (distance <= attackDistance)
            {
                if(timerForNextAttack <= 0)
                {
                    Attack();
                }
                else
                {
                    Idle();
                }
            }
            else
            {
                Run();
                Enemy.SetDestination(Player.position);
            }

        }
        //If player is not nearby, enemy will walk toward target point
        else
        {
            Walk();
            Enemy.SetDestination(EndPoint.position);
        }
    }

    //Check distance bewteen player and itself and enable isNearby and isAttack based on distance
    private void CheckDistacne()
    {
        distance = Vector3.Distance(Enemy.transform.position, Player.position);
        if (distance <= detectDistance)
        {
            isNearby = true;
        }
        else if (distance <= attackDistance)
        {
            isAttack = true;
        }
        else
        {
            isNearby = false;
            isAttack = false;
        }
    }

    //Walk animaiton
    private void Walk()
    {
        animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        Enemy.speed = walkSpeed;
    }

    //run animaiton
    private void Run()
    {
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        Enemy.speed = runSpeed;
    }

    //Idle animaiton
    private void Idle()
    {
        animator.SetTrigger("Idle");
    }

    //Attack animaiton
    private void Attack()
    {
        animator.SetTrigger("Attack");
        startCoolDown = true;
        timerForNextAttack = coolDown;
    }

    //This is the cooldown function it will check if enemy used attack function, if used, start count up.
    //When timer exceed 1.5 seconds, enemy are able to perform another attack and timer reset.
    private void CoolDown()
    {
        if (startCoolDown)
        {
            timerForNextAttack -= Time.deltaTime;
        }
        else if (timerForNextAttack <= 0)
        {
            startCoolDown = false;
        }
    }

    //Let player call this function to apply damage to enemy and play GetHit animation
    public void TakeDamage(float amount)
    {
        animator.SetTrigger("GetHit");
        health -= amount;
        Debug.Log("Enemy get hit");
    }

    //Check itself health and play Dead animation and destroy itself if health below 0
    private void CheckHealth()
    {
        healthBar.value = health / 100;
        if(health <= 0)
        {
            animator.SetTrigger("Dead");
            Destroy(this.gameObject, 3f);
        }
    }
}
