using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float damage;

    private void Start()
    {
        damage = this.GetComponentInParent<EnemyManagement>().damage;
    }

    //Enemy weapon collider collide with player. Call player takedamage function when player is not
    //defending
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<PlayerController>().isDefend)
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
