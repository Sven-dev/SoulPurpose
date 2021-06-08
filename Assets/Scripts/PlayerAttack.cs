using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int Damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it's enemy
        if (collision.tag == "Enemy")
        {
            //Damage it
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(Damage, transform.position);
        }
    }
}