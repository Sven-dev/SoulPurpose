using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private bool Attacking = false;
    [SerializeField] private bool HasWeapon = true;
    [SerializeField] private float TimeToThrow = 1f;
    [SerializeField] private float AttackTime = 0.1f;
    [SerializeField] private float Cooldown = 1f;
    [Space]
    [SerializeField] private List<GameObject> Hitboxes;
    [Space]
    [SerializeField] private Mover Mover;
    [SerializeField] private CustomAnimator Animator;

    private IEnumerator MeleeCoroutine;

    private bool OffCooldown = true;

    /// <summary>
    /// TO DO: The melee attack never gets reset, so it stays in the first attack animation after the first. cancel out of the coroutine.
    /// </summary>
    /*
    private void Start()
    {
        MeleeCoroutine = _MeleeAttack;
    }
    */

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
            print("Throw");
            RangedAttack();
        }
        else
        {
            print("Melee");
            MeleeAttack(Mover.Direction);
        }
    }

    /// <summary>
    /// Starts a melee attack, or a second melee attack if the player is already attacking.
    /// </summary>
    /// <param name="direction">The direction the player is facing.</param>
    private void MeleeAttack(int direction)
    {
        if (!Attacking)
        {
            Animator.MeleeAttack1();
        }
        else
        {
            Animator.MeleeAttack2();
        }

        if (direction == -1)
        {
            StartCoroutine(_MeleeAttack(Hitboxes[0]));
        }
        else if (direction == 1)
        {
            StartCoroutine(_MeleeAttack(Hitboxes[1]));
            //StopCoroutine(_MeleeAttack(Hitboxes[0]));
        }
    }

    private IEnumerator _MeleeAttack(GameObject Hitbox)
    {
        OffCooldown = false;
        Attacking = true;

        Hitbox.SetActive(true);
        yield return new WaitForSeconds(AttackTime);
        Hitbox.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        OffCooldown = true;

        //Set attacking to false a little later, so the player gets a chance to attack again
        yield return new WaitForSeconds(0.4f);
        Attacking = false;
    }

    private void RangedAttack()
    {
        //todo
        throw new System.Exception("Feature not implemented yet");
    }
}