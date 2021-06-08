using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile_Stuck : MonoBehaviour
{
    private Transform Player;
    private bool Hanging = false;

    private void Update()
    {
        
    }

    public void SetVariables(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Hanging)
        {
            Hanging = true;
            Player = collision.transform;

            PlayerStuff.Instance.Mover.Moving = false;

            PlayerStuff.Instance.Rigidbody.gravityScale = 0;
            PlayerStuff.Instance.Rigidbody.velocity = Vector3.zero;

            PlayerStuff.Instance.Jumper.Hanging = true;
            if (PlayerStuff.Instance.Jumper.Grounded)
            {
                Unhang();
            }
            else
            {
                PlayerStuff.Instance.Animator.Hang();

                Player.parent = transform;
                Player.position = transform.position;
                Player.position += Vector3.down * 1f;

                StartCoroutine(_Hang());
            }
        }
    }

    private IEnumerator _Hang()
    {
        while (true)
        {
            if (Input.GetKeyDown(Controls.Instance.Jump))
            {
                PlayerStuff.Instance.Jumper.Hanging = false;
                PlayerStuff.Instance.Jumper.Jump();
                break;
            }

            if (PlayerStuff.Instance.Jumper.Grounded)
            {
                break;
            }

            yield return null;
        }

        Unhang();
    }

    private void Unhang()
    {
        Hanging = false;
        PlayerStuff.Instance.Mover.Moving = true;
        PlayerStuff.Instance.Jumper.Hanging = false;

        Player.parent = null;
        PlayerStuff.Instance.Rigidbody.gravityScale = 2;

        PlayerStuff.Instance.Attacker.GetWeapon();
        if (Input.GetKey(Controls.Instance.RangedAttack))
        {
            PlayerStuff.Instance.Attacker.StartRangedAttack();
        }

        Destroy(gameObject);
    }
}
