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
    private Transform[] Feet;
    [SerializeField]
    private Transform Head;
    [SerializeField]
    private float CheckRadius;
    [SerializeField]
    private LayerMask GroundMask;

    private bool Grounded = true;

    [Space]
    [SerializeField]
    private float Force;
    [SerializeField]
    private Rigidbody2D Rigidbody;

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //Check if the player is currently touching the ground
        foreach (Transform foot in Feet)
        {
            Grounded = Physics2D.OverlapCircle(foot.position, CheckRadius, GroundMask);
        }

        //if the player is allowed to jump, and is pressing the jump button
        if (Grounded && Input.GetKeyDown(Controls.Instance.Jump))
        {
            Grounded = false;
            StartCoroutine(_Jump());
        }
    }

    /// <summary>
    /// Makes the player jump according to 
    /// </summary>
    /// <returns></returns>
    private IEnumerator _Jump()
    {
        float multiplier = 0.15f;
        float duration = 0;
        while (duration < JumpCurve.keys[JumpCurve.keys.Length - 1].time)
        {
            //If the jump multiplier isnt maxed out, and the player is holding the jump button
            if (multiplier < 1 && Input.GetKey(Controls.Instance.Jump))
            {
                //Increase the multiplier
                multiplier += Time.deltaTime * 5;
            }

            //Check if the player is right above a ceiling
            if (Physics2D.OverlapCircle(Head.position, CheckRadius, GroundMask))
            {
                //If so, start falling down
                break;
            }

            duration += Time.fixedDeltaTime;
            transform.Translate(Vector2.up * JumpCurve.Evaluate(duration) * multiplier);
            yield return new WaitForFixedUpdate();
        }
    }
}