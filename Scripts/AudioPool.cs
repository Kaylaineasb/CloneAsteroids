using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
     public static AudioPool Instance { get; private set; }

    [SerializeField] private AudioSource audioSourcePrefab; // Prefab de um AudioSource
    [SerializeField] private int poolSize = 10; // Tamanho do pool

    private Queue<AudioSource> audioPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePool();
    }

    private void InitializePool()
    {
        audioPool = new Queue<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
            audioSource.gameObject.SetActive(false);
            audioPool.Enqueue(audioSource);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (audioPool.Count > 0)
        {
            AudioSource audioSource = audioPool.Dequeue();
            audioSource.transform.position = position;
            audioSource.clip = clip;
            audioSource.gameObject.SetActive(true);
            audioSource.Play();

            StartCoroutine(ReturnToPool(audioSource, clip.length));
        }
    }

    private System.Collections.IEnumerator ReturnToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.Stop();
        audioSource.gameObject.SetActive(false);
        audioPool.Enqueue(audioSource);
    }
}
