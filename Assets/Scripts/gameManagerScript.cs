using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    public GameObject GameOverUi;
    public Player player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver() 
    {
        GameOverUi.SetActive(true);
    }

    public void restart() 
    {
        Debug.Log("restart");
        SceneManager.LoadScene("floor-0");
        player.GetComponent<Player>().isDead = false;

    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void quit()
    {
        Application.Quit(); 
    }
}
