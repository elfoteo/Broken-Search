using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100;
    private float health = 100;
    private int xpForNextLevel = 0;
    private int currentXp = 0;
    private int currentLevel = 0;
    public float interactDistanceThreshold = 3.0f;
    public GameObject NPC;
    public Image healthbar;
    public gameManagerScript gameManager;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        xpForNextLevel = GetXpForLevel(currentLevel);
    }

    private int GetXpForLevel(int currentLevel)
    {
        double a = 1.2F;
        double b = 0;
        double c = 5;
        return (int)(a * currentLevel * (currentLevel * .1) + b * currentLevel + c);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthbar == null)
        {
            return;
        }


        healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        
        
        if (currentXp >= xpForNextLevel)
        {
            currentXp -= xpForNextLevel;
            currentLevel++;
            xpForNextLevel = GetXpForLevel(currentLevel);
        }
        // Calculate the distance between the player and the NPC
        if (NPC != null)
        {
            float distance = Vector3.Distance(NPC.transform.position, transform.position);
            DialogueTrigger trigger = NPC.GetComponent<DialogueTrigger>();
            if (distance < interactDistanceThreshold)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (trigger != null)
                    {
                        if (!trigger.isDialogStarted())
                        {
                            trigger.TriggerDialogue();
                        }
                        else
                        {
                            trigger.NextDialogue();
                        }
                    }
                }
            }
            else
            {
                trigger.EndDialog();
            }
        }
        else
        {
            Debug.LogError("Add it to the inpsector of the Player");
        }
    }

    public void Damage(float amount)
    {
        this.health -= amount;
        if (health <= 0 && !isDead) 
        {
            isDead = true;
            gameManager.gameOver();

        }

    }

    internal void Pickup(string name)
    {
        Debug.LogWarning("Picked up " + name);
    }
}
