using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Projectile : MonoBehaviour
{
    [SerializeField] private Vector2 Direction;
    [SerializeField] private float Speed;
    [Space]
    [SerializeField] private Rigidbody2D Rigidbody;


    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
        Rigidbody.AddForce(Direction * Speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it's player
        //pick up sword

        //if it's enemy
        //Damage it

        //if it's a surface
            //if it's a wall
            //drop it down

            //if it's a floor
            //stick it in the ground
    }
}
