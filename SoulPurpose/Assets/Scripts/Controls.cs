using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that keeps track of keybinds
/// </summary>
public class Controls : MonoBehaviour
{
    public static Controls Instance;

    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Jump = KeyCode.W;

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