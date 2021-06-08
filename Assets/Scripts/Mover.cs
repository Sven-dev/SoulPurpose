using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the character left or right
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [Header("Walking")]
    [SerializeField] private float Speed;
    [SerializeField] private AnimationCurve SpeedCurve;

    [Header("Unity Components")]
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private CustomAnimator Animator;

    [HideInInspector] public int Direction = 1;
    [HideInInspector] public bool Moving = true;

    [SerializeField]private float Momentum = 0;
    private IEnumerator Coroutine;

    private void Start()
    {
        StartCoroutine(_Walk());
    }

    public bool Walking
    {
        get
        {
            if (Coroutine != null)
            {
                return true;
            }

            return false;
        }
    }

    private IEnumerator _Walk()
    {
        while (true)
        {
            //If the player is pressing the right button
            if (Input.GetKey(Controls.Instance.Right))
            {
                if (Momentum < 0)
                {
                    Momentum = 0;
                }

                if (Momentum < 1)
                {
                    Momentum += 5 * Time.deltaTime;
                }

                //Move right
                StartMove(Controls.Instance.Right, 1);
                Animator.Walk(1);
            }
            //If the player is pressing the left button
            else if (Input.GetKey(Controls.Instance.Left))
            {
                if (Momentum > 0)
                {
                    Momentum = 0;
                }

                if (Momentum > -1)
                {
                    Momentum -= 5 * Time.deltaTime;
                }
                Animator.Walk(-1);

                //Move left
                StartMove(Controls.Instance.Left, -1);
            }
            //If the player isn't pressing anything
            else
            {
                //slowly decrease momentum back to 0
                if (Momentum > 0.025f)
                {
                    Momentum -= 10 * Time.deltaTime;
                }
                else if (Momentum < -0.025f)
                {
                    Momentum += 10 * Time.deltaTime;
                }
                else
                {
                    Momentum = 0;
                }
            }

            if (Moving)
            {
                Rigidbody.velocity = new Vector2(Speed * SpeedCurve.Evaluate(Momentum), Rigidbody.velocity.y);
            }

            yield return null;
        }
    }

        /// <summary>
        /// Check the controls
        /// </summary>
        private void Update()
        {       
        /*
            //If the player is pressing the right button
            if (Input.GetKeyDown(Controls.Instance.Right))
            {
                //Move right
                StartMove(Controls.Instance.Right, 1);
            }
            //If the player is pressing the left button
            else if (Input.GetKeyDown(Controls.Instance.Left))
            {
                //Move left
                StartMove(Controls.Instance.Left, -1);
            }
        */
        }


        /// <summary>
        /// Starts the _Move() coroutine
        /// </summary>
        /// <param name="key">The key that's pressed</param>
        /// <param name="direction">The direction the object is moving in</param>
        private void StartMove(KeyCode key, int direction)
    {
        if (Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }

        Direction = direction;
        Coroutine = _Move(key, direction);
        StartCoroutine(Coroutine);
    }

    /// <summary>
    /// Moves the object with momentum, meaning the object speeds up and slows down.
    /// </summary>
    /// <param name="key">The key that's pressed</param>
    /// <param name="direction">The direction the object is moving in</param>
    private IEnumerator _Move(KeyCode key, int direction)
    {
        yield return null;
        Animator.Walk(direction);

        float momentum = 0;
        while (Input.GetKey(key) && momentum < 10)
        {
            if (Moving)
            {
                if (momentum < SpeedCurve.keys[1].time)
                {
                    //Speed the player up
                    Rigidbody.velocity = new Vector2(direction * (Speed * SpeedCurve.Evaluate(momentum)), Rigidbody.velocity.y);
                    momentum += Time.fixedDeltaTime;
                }
                else
                {
                    //Player moves at max speed
                    Rigidbody.velocity = new Vector2(direction * (Speed * SpeedCurve.Evaluate(0.5f)), Rigidbody.velocity.y);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        //When the player lets go of the button
        momentum = SpeedCurve.keys[SpeedCurve.keys.Length - 1].time;
        while (momentum < 1)
        {
            if (Moving)
            {
                //Slow the player down
                Rigidbody.velocity = new Vector2(direction * (Speed * SpeedCurve.Evaluate(momentum)), Rigidbody.velocity.y);
                momentum += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        //Make the player stand still completely (negates sliding of the rigidbody)
        Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        Animator.Idle(direction);

        Coroutine = null;
    }
}