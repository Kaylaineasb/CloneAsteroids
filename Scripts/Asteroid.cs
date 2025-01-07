using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;
    public float initialSize = 5.0f;
    public float minSize = 3.0f;
    public float sizeReduction = 1.0f;
    public GameObject asteroidPrefab;
    
    // Novo limite de tamanho para impedir divisão
    public float preventSplitSize = 2.5f;  // Tamanho em que a divisão é proibida

    private Rigidbody2D rb;

    // Variável para contar o número de divisões
    private int splitCount = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Se a direção não foi atribuída, atribui uma direção aleatória
        if (direction == Vector2.zero)
        {
            direction = Random.insideUnitCircle.normalized;
        }

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado para o asteroide!");
        }
        else
        {
            SetMovement(); // Apenas chama SetMovement depois que o Rigidbody2D é encontrado
        }

        SetSize(initialSize);
    }

    void Update()
    {
        // Verifica se o asteroide possui direção e se o Rigidbody2D foi atribuído corretamente
        if (direction != Vector2.zero && rb != null)
        {
            rb.velocity = direction * speed;  // Atribui velocidade ao Rigidbody2D para mover o asteroide
        }
        else
        {
            Debug.LogError("Direção ou Rigidbody2D não atribuídos corretamente.");
        }

        // Teleporte nas bordas da tela
        Vector3 pos = transform.position;
        if (pos.x > 10) pos.x = -10;
        if (pos.x < -10) pos.x = 10;
        if (pos.y > 6) pos.y = -6;
        if (pos.y < -6) pos.y = 6;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
            // Destroi a bala após o impacto
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage()
    {
        // Reduz o tamanho do asteroide
        float newSize = initialSize - sizeReduction;

        // Verifica se o novo tamanho é maior que o mínimo permitido e se ainda não atingiu o tamanho de divisão proibido
        if (newSize > minSize && newSize > preventSplitSize && splitCount < 2) // Limita as divisões a 2
        {
            SplitAsteroid(newSize);
        }
        else
        {
            DestroyAsteroid();
        }
    }

    public void SetSize(float size)
    {
        initialSize = size;
        transform.localScale = Vector3.one * size;

        // Ajusta o tamanho do collider ao novo tamanho
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = size / 2;
        }
    }

    private void SplitAsteroid(float newSize)
    {
        // Impede a divisão se o asteroide tiver atingido o tamanho mínimo ou o tamanho que não pode se dividir
        if (newSize <= minSize || newSize <= preventSplitSize || splitCount >= 2) 
        {
            return;
        }

        // Cria dois novos asteroides
        for (int i = 0; i < 2; i++)
        {
            GameManager.Instance.AddScore(10);
            GameObject newAsteroid = Instantiate(asteroidPrefab, transform.position, Quaternion.identity);

            // Atribuindo o Rigidbody2D
            Rigidbody2D newRb = newAsteroid.GetComponent<Rigidbody2D>();
            if (newRb != null)
            {
                // Define a velocidade do novo asteroide (caso queira)
                newRb.velocity = direction * speed;
            }
            else
            {
                Debug.LogError("O asteroide instanciado não possui Rigidbody2D!");
            }

            // Configura o novo asteroide
            Asteroid asteroidScript = newAsteroid.GetComponent<Asteroid>();
            if (asteroidScript != null)
            {
                asteroidScript.SetSize(newSize); // Define o tamanho reduzido
                asteroidScript.SetDirection(Random.insideUnitCircle.normalized); // Define uma nova direção aleatória
            }
            else
            {
                Debug.LogError("O prefab do asteroide não tem o script Asteroid anexado!");
            }

            // Garante que o collider está ativado
            Collider2D newCollider = newAsteroid.GetComponent<Collider2D>();
            if (newCollider != null)
            {
                newCollider.enabled = true; // Ativa o collider
            }
            else
            {
                Debug.LogError("O asteroide instanciado não possui Collider2D!");
            }
        }

        // Incrementa o contador de divisões
        splitCount++;
    }

    // Método para obter a direção do asteroide
    public Vector2 GetDirection()
    {
        return direction;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    public void SetMovement()
    {
        // Verifica se o Rigidbody2D está atribuído e define a velocidade
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    private void DestroyAsteroid()
    {
        // Adiciona pontuação e destrói o asteroide
        GameManager.Instance.AddScore(50);
        Destroy(gameObject);
    }
}
