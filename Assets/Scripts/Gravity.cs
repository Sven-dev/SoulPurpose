using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]
    private float GravityScale;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.down * GravityScale * Time.fixedDeltaTime);
    }
}