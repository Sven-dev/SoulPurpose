using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the character to jump
/// </summary>
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
    private Transform[] Feet;
    [SerializeField]
    private float CheckRadius;
    [SerializeField]
    private LayerMask GroundMask;

    private bool Grounded = true;
    private bool Liftoff = false;

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //if the player is allowed to jump, and is pressing the jump button
        if (Grounded && Input.GetKeyDown(Controls.Instance.Jump))
        {
            StartCoroutine(_LiftoffTimer(0.2f));
            StartCoroutine(_Jump());
        }
    }

    // Checks if the player is grounded
    void FixedUpdate()
    {
        // is the player not jumping
        Grounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        if (!Liftoff)
        {
            bool onground = false;
            foreach (Transform foot in Feet)
            {
                if (!onground && Physics2D.OverlapCircle(foot.position, CheckRadius, GroundMask))
                {
                    onground = true;
                }
            }

            return onground;
        }

        return false;      
    }

    /// <summary>
    /// Makes the player jump according to 
    /// </summary>
    /// <returns></returns>
    private IEnumerator _Jump()
    {
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

            //Check if the player is right above a ceiling or on the ground
            if (Physics2D.OverlapCircle(Head.position, CheckRadius, GroundMask))
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 2].time;
                print("bopped");
            }
            if (IsGrounded())
            {
                duration = JumpCurve.keys[JumpCurve.keys.Length - 1].time;
            }

            transform.Translate(Vector2.up * JumpCurve.Evaluate(duration) * multiplier);
            duration += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Turns liftoff on for one physics-update (prevents ground check during liftoff)
    /// </summary>
    /// <returns></returns>
    private IEnumerator _LiftoffTimer(float time)
    {
        Liftoff = true;
        while (time > 0)
        {
            time -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Liftoff = false;
    }
}