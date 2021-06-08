using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private List<Door> Doors;
    [SerializeField] private bool Pressed = false;
    [Space]
    [SerializeField] private SpriteRenderer Renderer;

    [SerializeField] private Sprite PressedSprite;
    [SerializeField] private Sprite UnpressedSprite;

    private List<Collider2D> CollidersInTrigger = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CollidersInTrigger.Contains(collision))
        {
            CollidersInTrigger.Add(collision);
        }

        if (CollidersInTrigger.Count == 1)
        {
            Pressed = true;
            Renderer.sprite = PressedSprite;

            foreach(Door d in Doors)
            {
                d.Toggle(Pressed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CollidersInTrigger.Contains(collision))
        {
            CollidersInTrigger.Remove(collision);
        }

        if (CollidersInTrigger.Count == 0)
        {
            Pressed = false;
            Renderer.sprite = UnpressedSprite;

            foreach (Door d in Doors)
            {
                d.Toggle(Pressed);
            }
        }
    }
}