using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManagerScript : MonoBehaviour
{
    public GameObject GameOverUi;
    public Player player;
    [SerializeField] public Rigidbody2D rb;


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
        rb.velocity = Vector3.zero;
    }

    public void restart() 
    {
        SceneManager.LoadScene("floor-0");
        player.isDead = false;
        
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
