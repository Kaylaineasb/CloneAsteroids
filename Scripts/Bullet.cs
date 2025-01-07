using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Remove o tiro quando sair da tela
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Asteroid"))
        {
            // Destroi o asteroide e a bala
            Destroy(collision.gameObject); // Destroi o asteroide
            Destroy(gameObject); // Destroi a bala
        }

        if (collision.CompareTag("Enemy"))
        {
            // Destroi o inimigo
            Destroy(collision.gameObject);
            // Destroi o projétil
            Destroy(gameObject);
            // Incrementa o score
            GameManager.Instance.AddScore(10);
        }
    }
}
