using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagement : MonoBehaviour
{
    private float timer = 60f;
    private bool startTimer = false;

    public TextMeshProUGUI enemyNumber;//TextMesh shows total number of enemy
    public TextMeshProUGUI countdown;//TextMesh shows remaining time for lose game

    private int enemyLeft;
    private int collideCounter;//variable to help count how many enemies inside the target range


    private void Start()
    {
        collideCounter = 0;//initial collidecounter
    }

    private void Update()
    {
        if (startTimer)//start count down when there are enemy inside the target range
        {
            Debug.Log(timer);
            timer -= Time.deltaTime;
            countdown.text = "Time Remaining: " + timer;
        }
        if (timer <= 0f)//Lose the game when timer goes to 0
        {
            SceneManager.LoadScene("Lose");
        }
        EnemyRemain();
    }

    //count up the number of enemy insider the target range and enable the timer
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            startTimer = true;
            collideCounter++;
        }

    }

    //if enemy left the target range, the function will check if there are other enemies inside the range
    //reset and disable the timer only there is no enemy left inside the target range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            collideCounter--;
            if (collideCounter == 0)
            {
                startTimer = false;
                timer = 60f;
            }
        }
    }

    private void EnemyRemain()
    {
        //Create a array to store Enemy gameObject. Based on the level, there will be a different number of enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyLeft = enemies.Length;
        enemyNumber.text = "Enemy Remaining:  " + enemyLeft;

        //If there is no enemy in the game, player win
        if (enemyLeft == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
