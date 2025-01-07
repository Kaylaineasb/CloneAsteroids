using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton para acessar globalmente
    private AudioSource audioSource;
    public AudioSource musicSource; // AudioSource para a música de fundo
    public AudioClip backgroundMusic; // Música de fundo

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        audioSource.PlayOneShot(clip, volume); // Reproduz sem interromper outros sons
    }
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true; // Certifique-se de que a música vai repetir
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
