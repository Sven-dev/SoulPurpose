using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile_Line : MonoBehaviour
{
    [SerializeField] private float Speed;
    [Space]
    [SerializeField] private Sprite Horizontal;
    [SerializeField] private Sprite Vertical;
    [SerializeField] private Sprite Diagonal;
    [Space]
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private BoxCollider2D PickupTrigger;
    [Space]
    [SerializeField] private SwordProjectile_Arc SwordArcPrefab;
    [SerializeField] private SwordProjectile_Stuck SwordStuckPrefab;

    private bool Moving = true;
    private Vector2 Direction;

    private void Start()
    {
        Invoke("EnableTrigger", 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Moving)
        {
            if (collision.tag == "Wall")
            {
                Moving = false;
                SwordArcPrefab.enabled = true;

                SwordProjectile_Arc arc = Instantiate(SwordArcPrefab, transform.position, Quaternion.identity);
                arc.SetVariables((int)Direction.x * -1);

                Destroy(gameObject);
            }
            else if (collision.tag == "WallSticky")
            {
                Moving = false;

                SwordProjectile_Stuck stuck = Instantiate(SwordStuckPrefab, transform.position, Quaternion.identity);
                stuck.SetVariables(transform.rotation.eulerAngles);

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Set the variables of the sword. Gets called when the object gets instantiated.
    /// </summary>
    /// <param name="vector2">The direction the player is looking.</param>
    public void SetVariables(Vector2 direction, Vector3 rotation)
    {
        Direction = direction;   
        transform.rotation = Quaternion.Euler(rotation);

        StartCoroutine(_Move(direction));
    }

    private IEnumerator _Move(Vector2 direction)
    {
        while (Moving)
        {
            Rigidbody.position += direction * Speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Rigidbody.velocity = Vector2.zero;
    }

    private void EnableTrigger()
    {
        PickupTrigger.enabled = true;
    }
}