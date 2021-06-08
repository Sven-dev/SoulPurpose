using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCrystal : MonoBehaviour
{
    [SerializeField] private float Cooldown = 2;
    [Space]
    [SerializeField] private Attacker Player;

    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private BoxCollider2D Collider;

    private Vector3 Top;
    private Vector3 Bottom;

    private float t = 0;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        Top = transform.position;
        Top.y += 0.25f;

        Bottom = transform.position;
        Bottom.y -= 0.25f;

        StartCoroutine(_Float());
    }

    IEnumerator _Float()
    {
        while (true)
        {
            Vector3 temp = transform.position;

            temp = Vector3.Slerp(Top, Bottom, t);
            transform.position = temp;

            t += direction * Time.deltaTime;

            if (t >= 1 || t <= 0)
            {
                direction *= -1;
            }
            yield return null;
        }
    }

    IEnumerator _cooldown()
    {
        Renderer.enabled = false;
        Collider.enabled = false;

        yield return new WaitForSeconds(Cooldown);

        Renderer.enabled = true;
        Collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Player.HasWeapon)
        {
            Destroy(GameObject.Find("SwordLine(Clone)"));
            Destroy(GameObject.Find("SwordArc(Clone)"));
            Destroy(GameObject.Find("SwordStuck(Clone)"));

            Player.GetWeapon();
        }

        StartCoroutine(_cooldown());
    }
}