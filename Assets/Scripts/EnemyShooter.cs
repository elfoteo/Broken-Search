using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, IDamageable
{
    [SerializeField] public float hoverHeightAnim = 0.75f; // Amplitude of the hover motion
    [SerializeField] public float hoverSpeed = 1.0f; // Speed of the hover motion
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private float shootCooldown = 3000f; // Cooldown in milliseconds
    [SerializeField] private float shootingRange = 10.0f; // Range within which the enemy will shoot
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject enemyJuicePrefab; // Prefab for EnemyJuice

    private float health;
    private Vector3 originalPosition;
    private float lastShootTime = 0f; // Last time the enemy shot

    // Start is called before the first frame update
    void Start()
    {
        // Store the original position of the enemy
        originalPosition = transform.position;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeightAnim;
        // Update bar
        bar.transform.localScale = new Vector3(health / maxHealth, bar.transform.localScale.y, bar.transform.localScale.z);
        // Update the position of the enemy
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

        // Check if the player is within range and it's time to shoot
        if (Time.time * 1000f - lastShootTime >= shootCooldown && IsPlayerInRange())
        {
            Shoot();
            lastShootTime = Time.time * 1000f; // Reset the last shoot time
        }
    }

    private bool IsPlayerInRange()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Return true if the player is within shooting range
        return distanceToPlayer <= shootingRange;
    }

    private void Shoot()
    {
        // Instantiate the bullet prefab
        GameObject projectile = Instantiate(bulletPrefab, transform.position, transform.rotation);
        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();

        if (enemyProjectile != null)
        {
            // Set the projectile's target and other properties
            enemyProjectile.PositionAndRotate(player.transform.position);
        }
        else
        {
            Debug.LogError("Projectile does not have an EnemyProjectile component!");
        }
    }

    private void Die()
    {
        // Spawn particles (not shown in this snippet)

        // Spawn 3 EnemyJuice instances with random velocity
        for (int i = 0; i < 3; i++)
        {
            if (enemyJuicePrefab != null)
            {
                GameObject juice = Instantiate(enemyJuicePrefab, transform.position, Quaternion.identity);

                // Get EnemyJuice component
                EnemyJuice juiceScript = juice.GetComponent<EnemyJuice>();
                if (juiceScript != null)
                {
                    // Set player reference
                    juiceScript.SetPlayer(player);

                    // Add slight random velocity
                    Rigidbody2D rb = juice.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        float randomAngle = Random.Range(0f, Mathf.PI * 2f); // Random angle in radians
                        float randomSpeed = Random.Range(3f, 6f); // Random speed
                        Vector2 randomVelocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomSpeed;
                        rb.velocity = randomVelocity;
                    }
                    else
                    {
                        Debug.LogError("Juice prefab does not have a Rigidbody2D component!");
                    }
                }
                else
                {
                    Debug.LogError("Juice prefab does not have an EnemyJuice component!");
                }
            }
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }


    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
}
