using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // Velocidade do projétil
    public GameObject player; // Referência ao jogador
    public Vector3 respawnPoint; // Ponto de respawn do jogador

    private Rigidbody2D rb; // Referência ao Rigidbody2D
private Vector2 direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ignorar colisões entre projéteis usando camadas (opcional)
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);

        // Configura a direção inicial para o jogador
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    void Update()
    {
        // Se o jogador não for atribuído ou destruído, destrua o projétil
        if (player == null)
        {
            Destroy(gameObject);
        }
        if (player != null)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Remove o projétil quando sair da tela
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Diminuir vida do jogador
            GameManager.Instance.DecreaseLife();

            // Revive o jogador se houver vidas
            if (GameManager.Instance.lives > 0)
            {
                RevivePlayer(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject); // Destroi o jogador
            }

            Destroy(gameObject); // Destroi o projétil
        }
    }

    void RevivePlayer(GameObject playerObject)
    {
        playerObject.transform.position = respawnPoint; // Reposiciona o jogador no ponto de respawn
        playerObject.SetActive(true); // Garante que o jogador esteja ativo
    }
}
