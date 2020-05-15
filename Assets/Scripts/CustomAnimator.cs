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
    [Space]
    [SerializeField] private Mover Mover;

    private bool Jumping = false;
    private bool Falling = false;

    /// <summary>
    /// Plays idle animations
    /// </summary>
    /// <param name="direction"> the direction the player is looking at</param>
    public void Idle(int direction)
    {
        SetDirection(direction);
        if (!Jumping || Falling)
        {
            Legs.Play("Feet Idle");
        }
    }

    /// <summary>
    /// Plays walk animations
    /// </summary>
    /// <param name="direction"></param>
    public void Walk(int direction)
    {
        SetDirection(direction);
        if (!Jumping || Falling)
        {
            Legs.Play("Feet Walk");
        }
    }

    /// <summary>
    /// Plays jump animations
    /// </summary>
    public void Jump()
    {
        Jumping = true;
        Legs.Play("Feet Jump");
        Torso.Play("Armed Jump");
    }

    public void Fall()
    {
        Torso.Play("Armed Fall");
    }

    /// <summary>
    /// Reverts back to walk/idle animations
    /// </summary>
    public void Land()
    {
        Jumping = false;
        Torso.Play("Armed Right Idle");

        if (Mover.Walking)
        {
            Legs.Play("Feet Walk");
        }
        else
        {
            Legs.Play("Feet Idle");
        }
    }

    /// <summary>
    /// Flips the sprite renderer based on the direction the player is looking at
    /// </summary>
    /// <param name="direction"></param>
    private void SetDirection(int direction)
    {
        switch(direction)
        {
            case 1:
                TorsoRenderer.flipX = false;
                LegsRenderer.flipX = false;
                break;
            case -1:
                TorsoRenderer.flipX = true;
                LegsRenderer.flipX = true;
                break;
            default:
                throw new System.Exception("Direction is not an acceptable value");
        }
    }
}