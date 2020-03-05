using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the character to jump
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve JumpCurve;
    [SerializeField]
    private Rigidbody2D Rigidbody;

    [Space]
    [Header("Ground & ceiling detection")]
    [SerializeField]
    private Transform Head;
    [SerializeField]
    private float HeadRadius = 0.1f;

    [Space]
    [SerializeField]
    private Transform Feet;
    [SerializeField]
    private float FeetRadius = 0.3f;
    [SerializeField]
    private LayerMask GroundMask;

    private bool Grounded = false;

    [Space]
    [Header("CoyoteTime")]
    [SerializeField]
    private float AirTime = 0.15f;

    private bool InCoyoteTime = false;
    private bool Jumping = false;

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //if the player is allowed to jump, and is pressing the jump button
        if (Grounded && Input.GetKeyDown(Controls.Instance.Jump))
        {
            StartCoroutine(_Jump());
        }
    }

    /// <summary>
    /// Checks if the player is grounded
    /// </summary>
    void FixedUpdate()
    {
        if (!InCoyoteTime)
        {
            bool onground = Physics2D.OverlapBox(Feet.position, new Vector2(Feet.lossyScale.x, Feet.lossyScale.y)/10.0f, 0, GroundMask);         
            if (Grounded && !onground)
            {
                //if the player just left the ground, give them some coyote time
                StartCoroutine(_CoyoteTime());
            }
            else
            {
                Grounded = onground;
            }
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
    /// Makes the player jump according to 
    /// </summary>
    /// <returns></returns>
    private IEnumerator _Jump()
    {
        Jumping = true;
        Grounded = false;

        float multiplier = 0.25f;
        float duration = 0;
        while (duration < JumpCurve.keys[JumpCurve.keys.Length - 1].time)
        {
            //If the jump multiplier isnt maxed out, and the player is holding the jump button
            if (multiplier < 1 && Input.GetKey(Controls.Instance.Jump))
            {
                //Increase the multiplier
                multiplier += Time.deltaTime * 5;
            }

            transform.Translate(Vector2.up * JumpCurve.Evaluate(duration) * multiplier);
            duration += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

            //Check if the player is right above a ceiling or on the ground
            if (Physics2D.OverlapBox(Head.position, new Vector2(Head.lossyScale.x, Head.lossyScale.y)/10.0f, 0, GroundMask))
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 2].time;
            }

            if (Grounded)
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 1].time;
            }
        }

        Jumping = false;
    }
}