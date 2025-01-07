using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   public GameObject pauseMenu; 

    void Start()
    {
        Time.timeScale = 1; 
    }

    public void PlayGame()
    {
        Time.timeScale = 1; // Retoma o jogo.
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // Pausa o jogo.
    }

    public void ReloadGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarrega a cena atual.
    }

}
