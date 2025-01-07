
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
 public float speed = 1f; // Velocidade da nave eixo y
  public float horizontalSpeed = 2f; // Velocidade no eixo X
    public GameObject projectilePrefab; // Prefab do projétil
    public float fireRate = 0.5f; // Intervalo entre tiros
    public float projectileSpeed = 10f; // Velocidade do projétil
    public GameObject player; // Referência ao jogador
     public float horizontalRange = 3f; // Amplitude do movimento horizontal

    private Vector3 startPosition;
    private float timeElapsed;


    private void Start()
    {
        startPosition = transform.position;
        // Inicia o disparo periódico
        InvokeRepeating(nameof(FireProjectile), fireRate, fireRate);
    }

    private void Update()
    {
          // Atualiza o tempo para calcular o movimento
        timeElapsed += Time.deltaTime;

        // Movimento descendente combinado com movimento horizontal em onda (senoidal)
        float horizontalOffset = Mathf.Sin(timeElapsed * horizontalSpeed) * horizontalRange;
        transform.position = new Vector3(startPosition.x + horizontalOffset, transform.position.y - speed * Time.deltaTime, transform.position.z);

        // Destruir a nave se sair da tela
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab != null)
    {
        
        int numProjectiles = 3;

        for (int i = 0; i < numProjectiles; i++)
        {
            // Calcular posição de disparo para que as balas saiam de diferentes pontos
            Vector3 offset = new Vector3((i - 1) * 0.5f, 0f, 0f);  // Desloca o ponto de disparo à esquerda ou à direita

            // Instancia o projétil em uma posição ajustada
            GameObject projectile = Instantiate(projectilePrefab, transform.position + offset, Quaternion.identity);
            EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();

            // Configura a velocidade do projétil para mover na direção da nave do jogador
            if (enemyProjectile != null)
            {
                enemyProjectile.speed = projectileSpeed;
            }
        }
    }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Quando o jogador colide com a nave inimiga
            GameManager.Instance.DecreaseLife();
            Destroy(gameObject); // Destroi a nave inimiga
        }
    }
}