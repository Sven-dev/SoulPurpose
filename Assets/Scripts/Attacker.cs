using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Header("Attack type deciding")] 
    [SerializeField] private float Cooldown = 1f;
    [SerializeField] public bool HasWeapon { get; private set; } = true;     //Does the player have their weapon

    [Header("Melee attack")]
    [SerializeField] private float Attacknumber = 1;    //What attack is getting used;
    [SerializeField] private float AttackTime = 0.1f;   //The amount of time the hitbox is activated
    [SerializeField] private List<BoxCollider2D> Hitboxes;

    [Header("Ranged attack")]
    [SerializeField] private SwordProjectile_Line SwordPrefab;
    [SerializeField] private SwordProjectile_Line SwordPrefabDiagonal;
    [SerializeField] private List<Transform> SwordSpawns;

    [Space]
    [SerializeField] private SwordProjectile_Stuck SwordStuck;
    [SerializeField] private Transform SwordStuckSpawn;

    [Header("Unity Components")]
    [SerializeField] private Mover Mover;
    [SerializeField] private CustomAnimator Animator;

    [SerializeField] private LayerMask WallMask;

    private IEnumerator MeleeCoroutine;
    private bool OffCooldown = true;
    private Transform SwordSpawn;
    private Vector2 Direction;
    private Vector3 Rotation;

    /// <summary>
    /// Checks if the player is pressing the attack button.
    /// </summary>
    private void Update()
    {
        if (HasWeapon && OffCooldown)
        {
            if (Input.GetKeyDown(Controls.Instance.MeleeAttack))
            {
                MeleeAttack(Mover.Direction);
            }
            else if (Input.GetKeyDown(Controls.Instance.RangedAttack))
            {
                StartRangedAttack();
            }
        }
    }

    public void StartRangedAttack()
    {
        StartCoroutine(_CheckButton());
    }

    /// <summary>
    /// Checks if the player is holding for a short of a long time.
    /// </summary>
    private IEnumerator _CheckButton()
    {
        //play sword hold animation
        Animator.StartCharge();

        //Check how long the button is held
        while(Input.GetKey(Controls.Instance.RangedAttack))
        {
            yield return null;
        }

        RangedAttack();
    }

    /// <summary>
    /// Starts a melee attack, or a second melee attack if the player is already attacking.
    /// </summary>
    /// <param name="direction">The direction the player is facing.</param>
    private void MeleeAttack(int direction)
    {
        //Select the right attack animation, based on the previous attack used.
        if (Attacknumber == 1)
        {
            Animator.MeleeAttack1();
            Attacknumber = 2;
        }
        else if (Attacknumber == 2)
        {
            Animator.MeleeAttack2();
            Attacknumber = 1;
        }

        //Stop the current attack coroutine to stop the animation from being reset.
        if (MeleeCoroutine != null)
        {
            StopCoroutine(MeleeCoroutine);
        }

        if (direction == -1)
        {
            MeleeCoroutine = _MeleeAttack(Hitboxes[0]);
            StartCoroutine(MeleeCoroutine);
        }
        else if (direction == 1)
        {
            MeleeCoroutine = _MeleeAttack(Hitboxes[1]);
            StartCoroutine(MeleeCoroutine);
        }
    }

    private IEnumerator _MeleeAttack(BoxCollider2D Hitbox)
    {
        OffCooldown = false;

        Hitbox.gameObject.SetActive(true);
        Hitbox.enabled = true;

        yield return new WaitForSeconds(AttackTime);

        Hitbox.enabled = false;
        Hitbox.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        OffCooldown = true;

        //Set attacking to false a little later, so the player gets a chance to attack again
        yield return new WaitForSeconds(0.4f);
        Attacknumber = 1;
    }

    private void RangedAttack()
    {
        SetAimDirection();

        Collider2D collision = Physics2D.OverlapBox(SwordSpawn.position, new Vector2(SwordSpawn.lossyScale.x, SwordSpawn.lossyScale.y) / 5.0f, 0, WallMask);

        if (collision != null)
        {
            if (collision.tag == "Wall")
            {
                Animator.StopCharge();
            }
            else if (collision.tag == "WallSticky")
            {
                SwordProjectile_Stuck stuck = Instantiate(SwordStuck, SwordStuckSpawn.position, Quaternion.identity);
                stuck.SetVariables(Rotation);

                Animator.StopCharge();
                HasWeapon = false;
                Animator.LoseSword();
            }
        }
        else
        {
            SwordProjectile_Line sword = Instantiate(SwordPrefab, SwordSpawn.position, Quaternion.identity);
            sword.SetVariables(Direction, Rotation);

            Animator.StopCharge();
            HasWeapon = false;
            Animator.LoseSword();
        }
    }

    public void GetWeapon()
    {
        HasWeapon = true;
        Animator.GetSword();
    }

    /// <summary>
    /// Decides the direction the player is aiming in by checking their inputs.
    /// </summary>
    /// <returns>The direction.</returns>
    private void SetAimDirection()
    {
        Direction = Vector2.zero;
        Rotation = Vector3.zero;

        //If player is looking right
        if (Mover.Direction > 0)
        {
            //If player is moving right
            if (Input.GetKey(Controls.Instance.Right))
            {
                Direction += Vector2.right;
                SwordSpawn = SwordSpawns[0];

                //If player is holding up
                if (Input.GetKey(Controls.Instance.Up))
                {
                    Direction += Vector2.up;
                    Rotation = Vector3.forward * 45;
                    SwordSpawn = SwordSpawns[1];
                }
                //If player is holding down
                else if (Input.GetKey(Controls.Instance.Down))
                {
                    Direction += Vector2.down;
                    Rotation = Vector3.forward * -45;
                    SwordSpawn = SwordSpawns[7];
                }
            }
            //If player is not moving right
            else
            {
                //If player is holding up
                if (Input.GetKey(Controls.Instance.Up))
                {
                    Direction += Vector2.up;
                    Rotation = Vector3.forward * 90;
                    SwordSpawn = SwordSpawns[2];
                }
                //If player is holding down
                else if (Input.GetKey(Controls.Instance.Down))
                {
                    Direction += Vector2.down;
                    Rotation = Vector3.forward * -90;
                    SwordSpawn = SwordSpawns[6];
                }
                //If player is not holding any button
                else
                {
                    Direction += Vector2.right;
                    SwordSpawn = SwordSpawns[0];
                }
            }
        }

        //If player is looking left
        if (Mover.Direction < 0)
        {
            //If player is moving right
            if (Input.GetKey(Controls.Instance.Left))
            {
                Direction += Vector2.left;
                Rotation = Vector3.forward * 180;
                SwordSpawn = SwordSpawns[4];

                //If player is holding up
                if (Input.GetKey(Controls.Instance.Up))
                {
                    Direction += Vector2.up;
                    Rotation = Vector3.forward * 135;
                    SwordSpawn = SwordSpawns[3];
                }
                //If player is holding down
                else if (Input.GetKey(Controls.Instance.Down))
                {
                    Direction += Vector2.down;
                    Rotation = Vector3.forward * -135;
                    SwordSpawn = SwordSpawns[5];
                }
            }
            //If player is not moving right
            else
            {
                if (Input.GetKey(Controls.Instance.Up))
                {
                    Direction += Vector2.up;
                    Rotation = Vector3.forward * 90;
                    SwordSpawn = SwordSpawns[2];
                }
                else if (Input.GetKey(Controls.Instance.Down))
                {
                    Direction += Vector2.down;
                    Rotation = Vector3.forward * -90;
                    SwordSpawn = SwordSpawns[6];
                }
                else
                {
                    Direction += Vector2.left;
                    Rotation = Vector3.forward * 180;
                    SwordSpawn = SwordSpawns[4];
                }
            }
        }
    }
}