using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            //destroy
            Destroy(gameObject);
        }

        //Iframes

        //Damage animation

    }

    private IEnumerator _Iframes()
    {
        yield return null;
    }
}
