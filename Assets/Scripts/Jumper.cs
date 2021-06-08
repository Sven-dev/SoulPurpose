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

    [Header("Jumping")]
    [SerializeField] private AnimationCurve JumpCurve;

    [Header("Ground & ceiling detection")]
    [SerializeField] private Transform Head;

    [Space]
    [SerializeField] private Transform Feet;
    [SerializeField] private LayerMask GroundMask;
    public bool Grounded { get; private set; } = false;

    [Header("CoyoteTime")]
    [SerializeField] private float AirTime = 0.15f;

    [Header("JumpReminder")]
    [SerializeField][Tooltip("Allows the player to jump after they pressed the button")]
    private float RemindTime = 0.1f;

    [HideInInspector] public bool Hanging = false;

    private bool InCoyoteTime = false;
    private bool Jumping = false;
    private bool JumpPrevented = false;
    private bool LongPressing = false;
    private bool Falling = false;
    private bool JustJumped = false;

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
                //Start a longpress timer
                StartCoroutine(_Longpress());
            }
        }

        //If the player touches the ground when the timer is still going
        if (LongPressing && Grounded)
        {
            StartCoroutine(_Jump());
        }
    }

    /// <summary>
    /// Checks if the player is grounded every physics-update (0.2 seconds)
    /// </summary>
    private void FixedUpdate()
    {
        if (!InCoyoteTime)
        {
            bool onground = GetGround();

            //If the player just left the ground
            if (Grounded && !onground)
            {             
                //give them some coyote time
                StartCoroutine(_CoyoteTime());
            }

            //If the player is in the air
            else if (!Grounded)
            {
                //Check if the player is falling
                if (!JustJumped && !Falling)
                {
                    if (Rigidbody.velocity.y < 0)
                    {
                        Animator.Fall();
                        Falling = true;
                    }
                }

                //Check if the player just landed
                if (onground)
                {
                    Grounded = onground;
                    Falling = false;
                    Animator.Land();
                }
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

    public void Jump()
    {
        StartCoroutine(_Jump());
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    private IEnumerator _Jump()
    {
        if (!Physics2D.OverlapBox(Head.position, new Vector2(Head.lossyScale.x, Head.lossyScale.y) / 10.0f, 0, GroundMask))
        {
            Grounded = false;
            Jumping = true;

            StartCoroutine(_JumpPrevent());
            StartCoroutine(_CoyoteTime());
            StartCoroutine(_JustJumped());

            yield return null;
            Animator.Jump();

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

                if (Hanging)
                {
                    break;
                }

                //Check if the player is right above a ceiling or on the ground
                if (Physics2D.OverlapBox(Head.position, new Vector2(Head.lossyScale.x, Head.lossyScale.y) / 10.0f, 0, GroundMask))
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
    private IEnumerator _Longpress()
    {
        LongPressing = true;
        yield return new WaitForSeconds(RemindTime);
        LongPressing = false;
    }

    /// <summary>
    /// Gets called when the player jumps, and makes sure the player doesn't land in the same frame.
    /// </summary>
    private IEnumerator _JustJumped()
    {
        JustJumped = true;
        yield return new WaitForFixedUpdate();
        JustJumped = false;
    }
}