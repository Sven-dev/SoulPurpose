using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    private Vector2 Direction = Vector2.right;
    private IEnumerator Look;

    private void Update()
    {
        if (Input.GetKey(Controls.Instance.Right))
        {
            if (Input.GetKey(Controls.Instance.Up))
            {

            }
            else if (Input.GetKey(Controls.Instance.Down))
            {

            }
            else
            {

            }
        }
        else if (Input.GetKey(Controls.Instance.Left))
        {
            if (Input.GetKey(Controls.Instance.Up))
            {

            }
            else if (Input.GetKey(Controls.Instance.Down))
            {

            }
            else
            {

            }
        }

        //If the player is trying to look up
        if (Input.GetKeyDown(Controls.Instance.Up))
        {
            //Stop the previous look coroutine
            if (Look != null)
            {
                StopCoroutine(Look);
            }

            //Start a new look coroutine
            Look = _Look(Controls.Instance.Up);
            StartCoroutine(Look);

            Direction = Vector2.right + Vector2.up;
        }
        //If the player is trying to look down
        else if (Input.GetKeyDown(Controls.Instance.Down))
        {
            //Stop the previous look animation
            if (Look != null)
            {
                StopCoroutine(Look);
            }

            Look = _Look(Controls.Instance.Down);
            StartCoroutine(Look);

            Direction = Vector2.right + Vector2.down;
        }
    }

    private IEnumerator _Look(KeyCode key)
    {
        while (Input.GetKey(key))
        {
            yield return null;
        }

        Direction = Vector2.right;
    }
}