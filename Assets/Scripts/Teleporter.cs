using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management


public class Teleporter : MonoBehaviour

{
    [SerializeField] private string sceneToLoad; // Field for the scene name
    [SerializeField] private GameObject player; // Reference to the player GameObject

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Detect collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            // Transition to the specified scene
            SceneManager.LoadScene(sceneToLoad);
            player.GetComponent<Player>().isDead = false;
        }
    }
}
