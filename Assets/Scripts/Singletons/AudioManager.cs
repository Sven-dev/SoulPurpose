using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip PlayerJump;

    //private List<AudioSource> 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(AudioClip clip, AudioType type)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
    }

    private IEnumerator _Play(AudioSource source)
    {
        yield return null;
    }

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

    public enum AudioType
    {
        Music,
        Sound_effect
    }
}
