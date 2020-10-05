using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{
    [SerializeField] private CustomAnimator Animator;

    private IEnumerator Look;

    // Update is called once per frame
    void Update()
    {
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

            //play the look animation
            Animator.LookUp();
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

            //play the look animation
            Animator.LookDown();
        }
    }

    private IEnumerator _Look(KeyCode key)
    {
        while (Input.GetKey(key))
        {
            yield return null;
        }

        Animator.LookForward();
    }
}