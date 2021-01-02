using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Projectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve PowerCurve;
    [SerializeField] private float Power;
    [Space]
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private Animator Animator;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private BoxCollider2D Trigger;

    [Header("Ground and ceiling detection")]
    [SerializeField] private Transform Groundcheck;
    [SerializeField] private LayerMask GroundMask;

    [Space]
    [SerializeField] private Transform Ceilingcheck;
    [SerializeField] private LayerMask CeilingMask;

    [Header("wall detection")]
    [SerializeField] private Transform LeftWallcheck;
    [SerializeField] private Transform RightWallcheck;
    [SerializeField] private LayerMask WallMask;

    [Space]
    [SerializeField] private int Damage = 1;

    private int Direction = 1;
    private bool Grounded = false;
    private bool WallCheck = true;

    private void Start()
    {
        Invoke("EnableTrigger", 0.1f);
    }

    private void FixedUpdate()
    {
        if (!Grounded)
        {
            //if the projectile has landed, turn it into the sword sprite
            if (Physics2D.OverlapBox(Groundcheck.position, new Vector2(Groundcheck.lossyScale.x, Groundcheck.lossyScale.y) / 10.0f, 0, GroundMask))
            {
                Grounded = true;

                //Rigidbody.isKinematic = true;
                Rigidbody.velocity = Vector2.zero;
                Rigidbody.angularVelocity = 0;

                Animator.Play("Land");
            }
            else if (WallCheck)
            {
                //if the projectile has hit a wall, make it go into the opposite direction
                if (Physics2D.OverlapBox(LeftWallcheck.position, new Vector2(LeftWallcheck.lossyScale.x, LeftWallcheck.lossyScale.y) / 10.0f, 0, WallMask)
                    ||
                    Physics2D.OverlapBox(RightWallcheck.position, new Vector2(RightWallcheck.lossyScale.x, RightWallcheck.lossyScale.y) / 10.0f, 0, WallMask))
                {
                    StartCoroutine(_DisableWallCheck());
                    FlipDirection();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if it's player
        if (collision.tag == "Player")
        {
            //pick up sword
            Attacker attacker = collision.GetComponent<Attacker>();
            attacker.GetWeapon();
            Destroy(gameObject);
        }

        //if it's enemy
        if (collision.tag == "Enemy")
        {
            //Damage it
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(Damage);
        }
    }

    /// <summary>
    /// Set the variables of the sword. Gets called when the object gets instantiated.
    /// </summary>
    /// <param name="vector2">The direction the player is looking.</param>
    public void SetVariables(int direction)
    {
        Direction = direction;
        //if the player is aiming left, make the sword move in the opposite direction
        if (direction < 0)
        {
            Renderer.flipX = true;
        }

        StartCoroutine(_Arc());
    }

    private IEnumerator _Arc()
    {
        bool ceilinghit = false;

        float progress = 0;
        while (progress < 1)
        {
            //If the sword has fallen on the ground, stop the arc
            if (Grounded)
            {
                break;
            }

            //If the sword hit a ceiling, reduce its arc
            if (Physics2D.OverlapBox(Ceilingcheck.position, new Vector2(Ceilingcheck.lossyScale.x, Ceilingcheck.lossyScale.y) / 10.0f, 0, CeilingMask))
            {
                progress = PowerCurve.keys[PowerCurve.keys.Length - 2].time;
                ceilinghit = true;
            }

            if (!ceilinghit)
            {
                Rigidbody.position += new Vector2(0, PowerCurve.Evaluate(progress)) * Power * Time.fixedDeltaTime;
                progress += Time.fixedDeltaTime;
            }

            Rigidbody.position += new Vector2(Direction, 0) * Power * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Disables the wall check for a couple of frames so it doesn't get triggered on the same wall again.
    /// </summary>
    private IEnumerator _DisableWallCheck()
    {
        WallCheck = false;
        yield return new WaitForSeconds(2);
        WallCheck = true;
    }

    private void EnableTrigger()
    {
        Trigger.enabled = true;
    }

    private void FlipDirection()
    {
        Direction = -Direction;
        Renderer.flipX = !Renderer.flipX;
    }
}