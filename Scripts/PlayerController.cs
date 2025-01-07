using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioClip shootSound;

    void Update()
    {
        // Movimentação para frente e rotação
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
        // Disparo
            if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        // Teleporte nas bordas da tela
    Vector3 pos = transform.position;
    if (pos.x > 10) pos.x = -10;
    if (pos.x < -10) pos.x = 10;
    if (pos.y > 6) pos.y = -6;
    if (pos.y < -6) pos.y = 6;
    transform.position = pos;
    }
    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        // Tocar som do tiro usando o AudioPool
        if (shootSound != null)
        {
            AudioPool.Instance.PlaySound(shootSound, firePoint.position);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Asteroid"))
    {
        FindObjectOfType<GameManager>().DecreaseLife();
        Destroy(collision.gameObject); // Destroi o asteroide que colidiu
    }
}

}
