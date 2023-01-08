using UnityEngine;
using System.Collections;
 
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource source;
    
    public AudioClip introClip;
    public AudioClip themeClip;

    private void Awake() => Instance = this;
    
    private void Start()
    {
        StartCoroutine(PlayTheme());
    }
 
    IEnumerator PlayTheme()
    {
        source.clip = introClip;
        source.Play();
        yield return new WaitForSeconds(source.clip.length);
        source.clip = themeClip;
        source.Play();
    }
}