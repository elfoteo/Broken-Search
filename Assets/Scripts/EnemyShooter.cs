using System;
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
        // TODO: Spawn particles
        // TODO: Drop enemy juice
        // After particles are spawned
        Destroy(gameObject);
        gameObject.transform.position = new Vector3(0, -1000, 0);
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Die();
        }
    }
}
