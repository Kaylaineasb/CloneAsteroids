
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Referência para o texto onde o high score será exibido

    void Start()
    {
       // Carrega as 3 melhores pontuações
        int highScore1 = PlayerPrefs.GetInt("HighScore1", 0);
        int highScore2 = PlayerPrefs.GetInt("HighScore2", 0);
        int highScore3 = PlayerPrefs.GetInt("HighScore3", 0);

        // Exibe os scores em um formato adequado
        highScoreText.text = $"1st: {highScore1}\n2nd: {highScore2}\n3rd: {highScore3}";
    }
    public void Recarregar(){
        SceneManager.LoadScene("play");
    }


//parte de configuração minha
    public void ResetHighScore()
{
    PlayerPrefs.SetInt("HighScore1", 0); // Reseta o High Score para 0
    PlayerPrefs.SetInt("HighScore2", 0);
    PlayerPrefs.SetInt("HighScore3", 0);
    PlayerPrefs.Save(); // Salva a mudança
    Debug.Log("High Score Resetado");
}
}
