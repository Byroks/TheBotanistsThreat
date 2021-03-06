using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 0.5f;
    public static bool GameIsPause = false;
    public GameObject pauseMenuUI;
    public GameObject GameOverUI;
    public GameObject NextLevelUI;
    public GameObject Charakter;

    void Update() {
        if (Input.GetKeyDown("r"))
            {
                Invoke("Restart", 0.2f);
            }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(pauseMenuUI != null){
                if(!GameIsPause){
                    Pause();
                }
                else{
                    Resume();
                }
            }
        }
    }

    void Pause(){
        GameIsPause = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(){
        GameIsPause = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;

    }

    public void QuitGame(){
        Application.Quit();
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void GameOver(){
        if(!gameHasEnded && GameOverUI != null){
            gameHasEnded = true;
            GameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void NextLevel(){
        NextLevelUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        Time.timeScale = 1;
    }

    void OnCollisionEnter2D (Collision2D other){
        if(other.gameObject.tag == "Player"){
            NextLevelUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
