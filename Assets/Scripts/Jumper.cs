using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the character to jump
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private CustomAnimator Animator;

    [Space][Header("Jumping")]
    [SerializeField] private AnimationCurve JumpCurve;

    [Space][Header("Ground & ceiling detection")]
    [SerializeField] private Transform Head;
    [SerializeField] private float HeadRadius = 0.1f;

    [Space]
    [SerializeField] private Transform Feet;
    [SerializeField] private float FeetRadius = 0.3f;
    [SerializeField] private LayerMask GroundMask;
    private bool Grounded = false;

    [Space][Header("CoyoteTime")]
    [SerializeField] private float AirTime = 0.15f;

    [Space][Header("JumpReminder")]
    [SerializeField][Tooltip("Allows the player to jump after they pressed the button")]
    private float RemindTime = 0.1f;

    private bool InCoyoteTime = false;
    private bool Jumping = false;
    private bool JumpPrevented = false;
    private bool JustJumped = false;
    private bool GroundCheckable = true;
    private bool Falling = false;

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //if the player is pressing the jump button
        if (Input.GetKeyDown(Controls.Instance.Jump))
        {
            //If the player is already grounded
            if (Grounded)
            {
                StartCoroutine(_Jump());
            }
            else
            {
                //Start a jump timer
                StartCoroutine(_JustJumped());
            }
        }

        //If the player touches the ground when the timer is still going
        if (JustJumped && Grounded)
        {
            StartCoroutine(_Jump());
        }

        if (GetGround() == true)
        {
            //if the player just landed
            Falling = false;
            Animator.Land();
        }

        //Check if the player is falling
        if (!Falling)
        {
            if (Rigidbody.velocity.y < -0.1f)
            {
                Animator.Fall();
                Falling = true;
            }
        }
    }

    /// <summary>
    /// Checks if the player is grounded every physics-update (0.2 seconds)
    /// </summary>
    void FixedUpdate()
    {
        if (!InCoyoteTime)
        {
            bool onground = GetGround();
            if (Grounded && !onground)
            {
                //if the player just left the ground, give them some coyote time
                StartCoroutine(_CoyoteTime());
            }
            else if (!JumpPrevented)
            {
                Grounded = onground;
            }
        }
    }

    /// <summary>
    /// Checks if the player is standing on ground
    /// </summary>
    private bool GetGround()
    {
        return Physics2D.OverlapBox(Feet.position, new Vector2(Feet.lossyScale.x, Feet.lossyScale.y) / 10.0f, 0, GroundMask);
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    private IEnumerator _Jump()
    {
        Grounded = false;
        Jumping = true;

        Animator.Jump();

        StartCoroutine(_JumpPrevent());
        StartCoroutine(_CoyoteTime());

        float multiplier = 0.3f;
        float duration = 0;

        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0);
        while (duration < JumpCurve.keys[JumpCurve.keys.Length - 1].time)
        {
            //If the jump multiplier isnt maxed out, and the player is holding the jump button
            if (multiplier < 1 && Input.GetKey(Controls.Instance.Jump))
            {
                //Increase the multiplier
                multiplier += Time.deltaTime * 5;
            }

            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, JumpCurve.Evaluate(duration) * multiplier);
            duration += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

            //Check if the player is right above a ceiling or on the ground
            if (Physics2D.OverlapBox(Head.position, new Vector2(Head.lossyScale.x, Head.lossyScale.y)/10.0f, 0, GroundMask))
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 1].time;
            }

            //Check if the player is on the ground again
            if (duration > 0.4f && Grounded)
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 1].time;
            }
        }


        Jumping = false;
    }

    /// <summary>
    /// Allows the player to jump when they've just left the platform
    /// </summary>
    private IEnumerator _CoyoteTime()
    {
        InCoyoteTime = true;
        yield return new WaitForSeconds(AirTime);
        InCoyoteTime = false;
        Grounded = false;
    }

    /// <summary>
    /// Prevents double jumping right due to coyote time
    /// </summary>
    private IEnumerator _JumpPrevent()
    {
        JumpPrevented = true;
        yield return new WaitForSeconds(AirTime);
        JumpPrevented = false;
    }

    /// <summary>
    /// Allows the player to jump for a little longer after pressing the button
    /// </summary>
    private IEnumerator _JustJumped()
    {
        JustJumped = true;
        yield return new WaitForSeconds(RemindTime);
        JustJumped = false;
    }
}