using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator Torso;
    [SerializeField] private Animator Legs;
    [Space]
    [SerializeField] private SpriteRenderer TorsoRenderer;
    [SerializeField] private SpriteRenderer LegsRenderer;

    [Header("Jump Squish")]
    [SerializeField] private float JumpAmount;
    [SerializeField] private float JumpTime;
    [Header("Land Squish")]
    [SerializeField] private float LandAmount;
    [SerializeField] private float LandTime;

    private string Lookangle = "Forward";
    private string Direction = "Right";
    private string Action = "Idle";

    //Action:
    //Animation window parameter. The action the player is currently performing.
    //0 = idle
    //1 = walking
    //2 = jumping

    /// <summary>
    /// Plays idle animations
    /// </summary>
    /// <param name="direction"> the direction the player is looking at</param>
    public void Idle(int direction)
    {
        Action = "Idle";

        SetDirection(direction);
        Legs.SetBool("Idle", true);
        Torso.SetBool("Walking", false);
    }

    private void Update()
    {
        print(TorsoRenderer.sprite.rect);
    }

    /// <summary>
    /// Plays walk animations
    /// </summary>
    /// <param name="direction"></param>
    public void Walk(int direction)
    {
        Action = "Walk";

        SetDirection(direction);
        Legs.SetBool("Idle", false);
        Torso.SetBool("Walking", true);
    }

    /// <summary>
    /// Plays jump animation
    /// </summary>
    public void Jump()
    {
        StartCoroutine(_Squish(Axis.X, JumpAmount, JumpTime));

        Action = "Jump";

        Torso.SetBool("Jumping", true);
        Legs.SetBool("Jumping", true);

        if (!Torso.GetBool("Melee Attacking"))
        {
            Torso.SetTrigger("Jump");
            Legs.SetTrigger("Jump");
        }
    }

    /// <summary>
    /// Gets called when the player is hanging in mid-air
    /// </summary>
    public void Hang()
    {
        Torso.SetBool("Hanging", true);
        Legs.SetBool("Hanging", true);
    }

    public void Fall()
    {
        Torso.SetBool("Falling", true);
        Legs.SetBool("Falling", true);
    }

    /// <summary>
    /// Reverts back to walk/idle animations.
    /// </summary>
    public void Land()
    {
        StartCoroutine(_Squish(Axis.Y, LandAmount, LandTime));

        Torso.SetBool("Jumping", false);
        Legs.SetBool("Jumping", false);

        Torso.SetBool("Hanging", false);
        Legs.SetBool("Hanging", false);

        Torso.SetBool("Falling", false);
        Legs.SetBool("Falling", false);
    }

    /// <summary>
    /// Handles the melee attack animation.
    /// </summary>
    public void MeleeAttack1()
    {
        Torso.SetTrigger("Melee Attack");
    }

    public void MeleeAttack2()
    {
        Torso.SetTrigger("Melee Attack 2");
    }

    public void RangedCharge()
    {
        Torso.SetBool("Ranged Attacking", true);
        Torso.SetTrigger("Ranged Charge");
    }

    public void LookUp()
    {
        Lookangle = "Up";
        Torso.SetInteger("Look Direction", 1);
    }

    public void LookDown()
    {
        Lookangle = "Down";
        Torso.SetInteger("Look Direction", -1);
    }

    public void LookForward()
    {
        Lookangle = "Forward";
        Torso.SetInteger("Look Direction", 0);
    }

    public void LoseSword()
    {
        Torso.SetTrigger("Lose Sword");
        Torso.SetBool("Armed", false);
        Torso.SetBool("Ranged Attacking", false);
    }

    public void GetSword()
    {
        Torso.SetTrigger("Get Sword");
        Torso.SetBool("Armed", true);
    }

    /// <summary>
    /// Flips the player character based on the direction the player is looking at.
    /// </summary>
    /// <param name="direction">The direction the player is looking at (-1 = left, 1 = right).</param>
    private void SetDirection(int direction)
    {
        switch (direction)
        {
            case 1:
                Direction = "Right";
                Torso.SetBool("Facing Right", true);
                LegsRenderer.flipX = false;

                break;
            case -1:
                Direction = "Left";
                Torso.SetBool("Facing Right", false);
                LegsRenderer.flipX = true;

                break;
            default:
                throw new System.Exception("Direction is not an acceptable value");
        }
    }

    /// <summary>
    /// Squishes the spriterenderer container to emphathize animations.
    /// </summary>
    /// <param name="axis">The axis the object needs to be squished on (X, Y or Z).</param>
    /// <param name="amount">The amount of squish the object should have, on a scale between 0 and 1.</param>
    /// <param name="length">The amount of time an object needs to be squished for.</param>
    private IEnumerator _Squish(Axis axis, float amount, float length)
    {
        Vector3 squishVector;
        switch (axis)
        {
            case Axis.X:
                squishVector = Vector3.one - Vector3.right * (amount);
                break;
            case Axis.Y:
                squishVector = Vector3.one - Vector3.up * (amount);
                break;
            default:
                throw new System.Exception("axis not implemented!");
        }

        float progress = 0;
        while (progress < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, squishVector, progress);
            progress += (1 / length * Time.deltaTime) * 2;
            yield return null;
        }

        progress = 0;
        while (progress < 1)
        {
            transform.localScale = Vector3.Lerp(squishVector, Vector3.one, progress);
            progress += (1 / length * Time.deltaTime) * 2;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}

public enum Axis
{
    X,
    Y
}