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
    public Image juiceBar;
    public gameManagerScript gameManager;
    public bool isDead;
    public GameObject uiUpgrade;
    public PlayerDataSO playerData; // Reference to PlayerDataSO
    public AudioSource damageAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        xpForNextLevel = GetXpForLevel(currentLevel);

        // Subscribe to the OnHealthChanged event from PlayerDataSO
        playerData.OnHealthChanged.AddListener(UpdateHealthUI);
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
            uiUpgrade.SetActive(true);
            currentXp -= xpForNextLevel;
            currentLevel++;
            xpForNextLevel = GetXpForLevel(currentLevel);
        }

        juiceBar.fillAmount = Mathf.Clamp((float)currentXp / xpForNextLevel, 0, 1);
        if (juiceBar == null)
        {
            return;
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
            Debug.LogError("Add it to the inspector of the Player");
        }
    }

    public void Damage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (amount > 0 && damageAudioSource != null)
        {
            damageAudioSource.Play();
        }
        else
        {
            if (damageAudioSource == null)
            {
                Debug.LogError("Damage audio source is not assigned!");
            }
        }

        // Update PlayerDataSO with current health value
        playerData.PlayerHealth = health;
        
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

    internal void AddJuice(int v)
    {
        currentXp += v;
    }

    internal void IncreaseHitSpeed(int v)
    {
        this.gameObject.GetComponent<Weapon>().ReduceCooldown(v);
    }

    internal void IncreaseDamageDelt(float v)
    {
        this.gameObject.GetComponent<Weapon>().AddBonusDamage(v);
    }

    private void UpdateHealthUI()
    {
        if (healthbar != null)
        {
            healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        }
    }

    private void OnDestroy()
    {
        // Clean up event listener to prevent memory leaks
        playerData.OnHealthChanged.RemoveListener(UpdateHealthUI);
    }
}
