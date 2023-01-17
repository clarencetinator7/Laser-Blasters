using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float delayInSeconds;

   public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        FindObjectOfType<MusicPlayer>().DestroyOnMenu();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
        
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("Game Over");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
