using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the character to jump
/// </summary>
public class Jumper : MonoBehaviour
{
    [SerializeField]
    private float Force;
    [SerializeField]
    private Rigidbody2D Rigidbody;

    private bool Jumpable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Check the controls
    /// </summary>
    private void Update()
    {
        //if the player is allowed to jump, and is pressing the jump button
        if (Jumpable && Input.GetKeyDown(Controls.Instance.Jump))
        {
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(Vector2.up * Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Jumpable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Jumpable = false;
    }

    /*
    private void StartJump()
    {
        StartCoroutine(_Jump());
    }

    private IEnumerator _Jump()
    {
        
    }
    */
}