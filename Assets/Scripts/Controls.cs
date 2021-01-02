using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that keeps track of keybinds
/// </summary>
public class Controls : MonoBehaviour
{
    public static Controls Instance;

    //walking
    public KeyCode Left =           KeyCode.A;
    public KeyCode Right =          KeyCode.D;
    public KeyCode Up =             KeyCode.W;
    public KeyCode Down =           KeyCode.S;

    public KeyCode Jump =           KeyCode.RightShift;
    public KeyCode MeleeAttack =    KeyCode.Mouse0;
    public KeyCode RangedAttack =   KeyCode.Mouse1;

    /// <summary>
    /// Creates a singleton if it doesn't exist already
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}