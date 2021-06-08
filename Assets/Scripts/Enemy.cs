using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Health;

    [Header("Damage")]
    [SerializeField] private float Iframes;
    [SerializeField] private Color DamageColor;
    private Color DefaultColor;

    [Header("Knockback")]
    [SerializeField] private float Power;
    [SerializeField] private AnimationCurve KnockbackCurve;
    [Space]
    [SerializeField] private BoxCollider2D Hitbox;
    [SerializeField] private Rigidbody2D Rigidbody;
    [SerializeField] private SpriteRenderer Renderer;

    // Start is called before the first frame update
    void Start()
    {
        DefaultColor = Renderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 direction)
    {
        Health -= damage;
        if (Health <= 0)
        {
            //destroy
            Destroy(Renderer.gameObject);
        }
        else
        {
            //Iframes
            StartCoroutine(_Iframes());

            //Damage animation

            //Knockback
            if (transform.position.x > direction.x)
            {
                //Knock back to the right
                StartCoroutine(_Knockback(1));
            }
            else
            {
                //Knock back to the left
                StartCoroutine(_Knockback(-1));
            }
        }
    }

    private IEnumerator _Iframes()
    {
        Hitbox.enabled = false;
        Renderer.color = DamageColor;

        float CurrentIframes = 0;
        while (CurrentIframes <= Iframes)
        {
            Renderer.enabled = !Renderer.enabled;

            CurrentIframes += 0.1f;
            yield return new WaitForSeconds(0.1f);

        }

        Renderer.enabled = true;
        Renderer.color = DefaultColor;
        Hitbox.enabled = true;
    }

    private IEnumerator _Knockback(float direction)
    {
        float duration = 0;
        float maxduration = KnockbackCurve.keys[KnockbackCurve.keys.Length - 1].time;
        while (duration <= maxduration)
        {
            Rigidbody.velocity = new Vector2(direction * duration * Power, KnockbackCurve.Evaluate(duration) * Power);

            duration += Time.fixedDeltaTime * 10;
            yield return new WaitForFixedUpdate();
        }
    }
}