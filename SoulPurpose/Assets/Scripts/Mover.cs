using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the character left or right
/// </summary>
public class Mover : MonoBehaviour
{
    [SerializeField]
    private float Speed;  
    [SerializeField]
    private AnimationCurve SpeedCurve;

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //If the player is pressing the right button
        if (Input.GetKeyDown(Controls.Instance.Right))
        {
            //Move right
            StartMove(Controls.Instance.Right, Vector2.right);
        }
        //If the player is pressing the left button
        else if (Input.GetKeyDown(Controls.Instance.Left))
        {
            //Move left
            StartMove(Controls.Instance.Left, Vector2.left);
        }
    }

    /// <summary>
    /// Starts the _Move() coroutine
    /// </summary>
    /// <param name="key">The key that's pressed</param>
    /// <param name="direction">The direction the object is moving in</param>
    private void StartMove(KeyCode key, Vector2 direction)
    {
        StartCoroutine(_Move(key, direction));
    }

    /// <summary>
    /// Moves the object with momentum, meaning the object speeds up and slows down.
    /// </summary>
    /// <param name="key">The key that's pressed</param>
    /// <param name="direction">The direction the object is moving in</param>
    /// <returns></returns>
    private IEnumerator _Move(KeyCode key, Vector2 direction)
    {
        float momentum = 0;

        //While the player is holding the button
        while (Input.GetKey(key) && momentum < 10)
        {
            //Speed the player up
            if (momentum < SpeedCurve.keys[1].time)
            {
                transform.Translate(direction * (Speed * SpeedCurve.Evaluate(momentum)) * Time.fixedDeltaTime);
                momentum += Time.fixedDeltaTime;
            }
            //Player moves at max speed
            else
            {
                transform.Translate(direction * (Speed * SpeedCurve.Evaluate(0.5f)) * Time.fixedDeltaTime);
            }

            yield return new WaitForFixedUpdate();
        }

        //When the player lets go of the button
        momentum = SpeedCurve.keys[SpeedCurve.keys.Length - 1].time;
        //Slow the player down
        while (momentum < 1)
        {
            transform.Translate(direction * (Speed * SpeedCurve.Evaluate(momentum)) * Time.fixedDeltaTime);
            momentum += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}