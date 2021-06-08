using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool Open;

    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private BoxCollider2D Collider;

    public void Toggle(bool state)
    {
        Open = state;
        if(Open)
        {
            Renderer.enabled = false;
            Collider.enabled = false;
        }
        else
        {
            Renderer.enabled = true;
            Collider.enabled = true;
        }
    }
}
