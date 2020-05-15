using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private bool HasWeapon = true;
    [SerializeField] private float TimeToThrow = 1f;
    [SerializeField] private float AttackTime = 0.1f;
    [SerializeField] private float Cooldown = 1f;
    [Space]
    [SerializeField] private List<GameObject> Hitboxes;
    [Space]
    [SerializeField] private Mover Mover;
    [SerializeField] private CustomAnimator Animator;

    private bool OffCooldown = true;

    /// <summary>
    /// Checks if the player is pressing the attack button
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(Controls.Instance.Attack) && HasWeapon && OffCooldown)
        {
            StartCoroutine(_CheckButton());
        }
    }

    /// <summary>
    /// Checks if the player is holding for a short of a long time
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
    /// Starts a melee attack 
    /// </summary>
    /// <param name="direction">The direction the player is facing</param>
    private void MeleeAttack(int direction)
    {
        Animator.MeleeAttack();
        StartCoroutine(_Cooldown());

        if (direction == -1)
        {
            StartCoroutine(_MeleeAttack(Hitboxes[0]));
        }
        else if (direction == 1)
        {
            StartCoroutine(_MeleeAttack(Hitboxes[1]));
        }
    }

    private IEnumerator _MeleeAttack(GameObject Hitbox)
    {
        Hitbox.SetActive(true);
        yield return new WaitForSeconds(AttackTime);
        Hitbox.SetActive(false);
    }

    private void RangedAttack()
    {
        //todo
        throw new System.Exception("Feature not implemented yet");
    }

    private IEnumerator _Cooldown()
    {
        OffCooldown = false;
        yield return new WaitForSeconds(Cooldown);
        OffCooldown = true;
    }
}