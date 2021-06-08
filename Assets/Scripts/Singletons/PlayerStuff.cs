using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStuff : MonoBehaviour
{
    public static PlayerStuff Instance;

    public Mover Mover;
    public Jumper Jumper;
    public Attacker Attacker;
    public Rigidbody2D Rigidbody;
    public CustomAnimator Animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        GameObject player = GameObject.FindWithTag("Player");

        Mover = player.GetComponent<Mover>();
        Jumper = player.GetComponent<Jumper>();
        Attacker = player.GetComponent<Attacker>();
        Rigidbody = player.GetComponent<Rigidbody2D>();
        Animator = player.GetComponentInChildren<CustomAnimator>();
    }
}