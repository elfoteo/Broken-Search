using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private float shootCooldown = 3000f; // Cooldown in milliseconds
    [SerializeField] private float shootingRange = 10.0f; // Range within which the enemy will shoot
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject bar;
    [SerializeField] private float gravityScale = 1.0f; // Gravity scale for the projectile
    [SerializeField] private float landingTime = 1.0f; // Time for the projectile to land
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private float health;
    private float lastShootTime = 0f; // Last time the enemy shot

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Update bar
        bar.transform.localScale = new Vector3(health / maxHealth, bar.transform.localScale.y, bar.transform.localScale.z);

        // Check if the player is within range and it's time to shoot
        if (Time.time * 1000f - lastShootTime >= shootCooldown && IsPlayerInRange())
        {
            Shoot();
            lastShootTime = Time.time * 1000f; // Reset the last shoot time
        }

        // Flip the sprite to face the player
        FlipSpriteToFacePlayer();
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
        GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Set gravity scale for the projectile
            rb.gravityScale = gravityScale;

            // Calculate the initial velocity required to hit the player
            Vector2 start = transform.position;
            Vector2 end = player.transform.position;
            Vector2 toTarget = end - start;
            float distance = toTarget.magnitude;

            // Calculate the initial velocity to land the projectile on the player in the given landing time
            Vector2 initialVelocity = new Vector2(
                toTarget.x / landingTime,
                (toTarget.y / landingTime) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * gravityScale * landingTime
            );

            // Apply velocity to the projectile
            rb.velocity = initialVelocity;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody2D component!");
        }
    }

    private void FlipSpriteToFacePlayer()
    {
        if (player != null)
        {
            // If the player is to the right of the spider, make sure the sprite is not flipped
            if (player.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            // If the player is to the left of the spider, flip the sprite
            else if (player.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void Die()
    {
        // TODO: Spawn particles
        // TODO: Drop enemy juice
        // After particles are spawned
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
