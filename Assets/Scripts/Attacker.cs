using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Header("Attack type deciding")]
    [SerializeField] private bool HasWeapon = true;     //Does the player have their weapon
    [SerializeField] private float Cooldown = 1f;
    [SerializeField] private float TimeToThrow = 1f;    //If the player holds the button for more than this, the attack becomes a throw instead of a melee

    [Header("Melee attack")]
    [SerializeField] private float Attacknumber = 1;    //What attack is getting used;
    [SerializeField] private float AttackTime = 0.1f;   //The amount of time the hitbox is activated
    [SerializeField] private List<GameObject> Hitboxes;

    [Header("Ranged attack")]
    [SerializeField] private Sword_Projectile SwordPrefab;
    [SerializeField] private Transform SwordSpawn;

    [Header("Unity Components")]
    [SerializeField] private Mover Mover;
    [SerializeField] private Looker Looker;
    [SerializeField] private CustomAnimator Animator;

    private IEnumerator MeleeCoroutine;
    private bool OffCooldown = true;

    /// <summary>
    /// Checks if the player is pressing the attack button.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(Controls.Instance.Attack) && HasWeapon && OffCooldown)
        {
            StartCoroutine(_CheckButton());
        }
    }

    /// <summary>
    /// Checks if the player is holding for a short of a long time.
    /// </summary>
    private IEnumerator _CheckButton()
    {
        //Check how long the button is held
        float HeldTime = 0;
        while(Input.GetKey(Controls.Instance.Attack))
        {
            HeldTime += Time.deltaTime;
            yield return null;
        }

        //If the button is held long enough
        if (HeldTime > TimeToThrow)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack(Mover.Direction);
        }
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

    private IEnumerator _MeleeAttack(GameObject Hitbox)
    {
        OffCooldown = false;

        Hitbox.SetActive(true);
        yield return new WaitForSeconds(AttackTime);
        Hitbox.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        OffCooldown = true;


        //Set attacking to false a little later, so the player gets a chance to attack again
        yield return new WaitForSeconds(0.4f);

        Attacknumber = 1;
    }

    private void RangedAttack()
    {
        Sword_Projectile sword = Instantiate(SwordPrefab, SwordSpawn.position, Quaternion.identity);
        sword.SetVariables(Mover.Direction);
    }
}