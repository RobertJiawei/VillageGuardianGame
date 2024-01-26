using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        //Get the default damage amount by get damage variable in playercontroller component
        damage = this.GetComponentInParent<PlayerController>().damage;
    }

    //Player weapon collider collide with enemy, call enemy take damage function
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManagement>().TakeDamage(damage);
        }
    }
}
