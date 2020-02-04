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
        int momentum = 0;

        //startup
        while (Input.GetKey(key) && momentum < 10)
        {
            transform.Translate(direction * (Speed * 0.1f * momentum) * Time.deltaTime);
            momentum++;
            yield return new WaitForFixedUpdate();
        }

        //full speed
        while (Input.GetKey(key))
        {
            transform.Translate(direction * Speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        //slowdown
        while (momentum > 0)
        {
            transform.Translate(direction * (Speed * 0.1f * momentum) * Time.deltaTime);
            momentum--;
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Moves the object
    /// </summary>
    /// <param name="direction">The direction the object is moving in</param>
    /// <param name="speed">The speed the object is moving at</param>
    private void Move(Vector2 direction, float speed)
    {
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}