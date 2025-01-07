using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static GameManager Instance { get; private set; }
    public int score = 0;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public int lives = 3;

    public AudioClip gameOverSound;
    public AudioClip explosionSound; // Som de explosão
    public AudioManager audioManager;
    public GameObject player;

    // Configurações da nave inimiga
    public GameObject enemyShipPrefab;
    public float enemySpawnIntervalMin = 10f; // Intervalo mínimo entre spawns
    public float enemySpawnIntervalMax = 20f; // Intervalo máximo entre spawns
    private bool isGameOver = false;

    //spawn asteroid
     public GameObject asteroidPrefab;
    public float spawnRate = 8f;
    public float spawnDistance = 10f;



    private void Awake()
    {
        //singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemyShip());
         InvokeRepeating("SpawnAsteroid", 0f, spawnRate);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        Debug.Log("Pontuação: " + score);
    }
    public void DecreaseLife()
{
    lives--;
    livesText.text = "Lives: " + lives;

    if (lives <= 0)
    {
        GameOver();
    }
}

void GameOver()
{
   // Carrega as 3 melhores pontuações
    int highScore1 = PlayerPrefs.GetInt("HighScore1", 0);
    int highScore2 = PlayerPrefs.GetInt("HighScore2", 0);
    int highScore3 = PlayerPrefs.GetInt("HighScore3", 0);

    // Adiciona a pontuação atual na lista
    int[] scores = new int[] { highScore1, highScore2, highScore3, score };

    // Ordena os scores em ordem decrescente
    System.Array.Sort(scores);
    System.Array.Reverse(scores);

    // Salva os 3 maiores scores
    PlayerPrefs.SetInt("HighScore1", scores[0]);
    PlayerPrefs.SetInt("HighScore2", scores[1]);
    PlayerPrefs.SetInt("HighScore3", scores[2]);
    PlayerPrefs.Save(); // Salva as mudanças

    // Lógica de Game Over
    if (audioManager != null)
    {
        audioManager.StopBackgroundMusic();
    }

    if (explosionSound != null)
    {
        AudioManager.Instance.PlaySound(explosionSound);
    }

    if (player != null)
    {
        player.SetActive(false);
    }

    if (gameOverSound != null)
    {
        AudioManager.Instance.PlaySound(gameOverSound);
    }

    Debug.Log("Game Over");
    gameOverText.gameObject.SetActive(true);
    SceneManager.LoadScene("ScoreScene");
}

public void RestartGame()
{
    Time.timeScale = 1; // Reseta a pausa
    lives = 3; // Reseta as vidas
    score = 0; // Reseta a pontuação
    livesText.text = "Lives: " + lives;
    scoreText.text = "Score: " + score;
    gameOverText.gameObject.SetActive(false); // Esconde o texto de Game Over
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
 // Corrotina para gerar a nave inimiga
    private IEnumerator SpawnEnemyShip()
    {
        while (!isGameOver)
        {
            float waitTime = Random.Range(enemySpawnIntervalMin, enemySpawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            if (!isGameOver)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 6f, 0f); // Posição aleatória no topo
Instantiate(enemyShipPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
    void SpawnAsteroid()
{
    Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
    GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

    float randomSize = Random.Range(3.0f, 5.0f);
    Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
    if (asteroidScript != null)
    {
        asteroidScript.SetSize(randomSize);  // Define o tamanho aleatório
        asteroidScript.SetDirection(Random.insideUnitCircle.normalized);  // Direção aleatória
        asteroidScript.SetMovement();  // Inicializa o movimento
    }
    else
    {
        Debug.LogError("Prefab do asteroide não possui o script Asteroid!");
    }
}
}
