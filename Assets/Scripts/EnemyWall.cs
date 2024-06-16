using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWall : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private int shootCooldownMax = 3000; // Cooldown in milliseconds
    [SerializeField] private int shootCooldownMin = 2000; // Cooldown in milliseconds
    [SerializeField] private float shootingRange = 11.0f; // Range within which the enemy will shoot
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject deathParticles;
    private float health;
    private int? shootCooldown;
    private static System.Random random = new System.Random();

    private float lastShootTime = 0f; // Last time the enemy shot

    private enum Attacks
    {
        NORMAL_SHOT,
        SPIKES_TOP,
        SPIKES_BOTTOM
    }

    // Start is called before the first frame update
    void Start()
    {
        // Store the original position of the enemy
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Update bar
        bar.transform.localScale = new Vector3(health / maxHealth / 4, bar.transform.localScale.y, bar.transform.localScale.z);
        if (shootCooldown == null)
        {
            
            shootCooldown = random.Next(shootCooldownMin, shootCooldownMax);
        }
        // Check if the player is within range and it's time to shoot
        if (Time.time * 1000f - lastShootTime >= shootCooldown && IsPlayerInRange())
        {
            Attacks[] values = (Attacks[])Enum.GetValues(typeof(Attacks));
            Attacks choosenAttack = values[random.Next(values.Length)];
            Shoot(choosenAttack);
            lastShootTime = Time.time * 1000f; // Reset the last shoot time
            shootCooldown = null;
        }
    }

    private bool IsPlayerInRange()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Return true if the player is within shooting range
        return distanceToPlayer <= shootingRange;
    }

    private void Shoot(Attacks choosenAttack)
    {
        switch (choosenAttack)
        {
            case Attacks.NORMAL_SHOT:
                // Create a bullet that goes towards the player
                InstantiateBulletAndRotate(transform.position, player.transform.position);
                break;
            case Attacks.SPIKES_TOP:
                for (int i = 0; i < 5; i++)
                {
                    float x = i*2;
                    float y = i * 0.5f;
                    Vector3 startPos = new Vector3(x, y, transform.position.z);
                    Vector3 target = new Vector3(x, y-5, transform.position.z);
                    InstantiateBulletAndRotate(startPos, target);
                }
                break;
            case Attacks.SPIKES_BOTTOM:
                for (int i = 0; i < 15; i++)
                {
                    float x = i * 2 + 1 + transform.position.x;
                    float y = i * 0.5f + 1 + transform.position.y;
                    Vector3 startPos = new Vector3(x, y, transform.position.z);
                    Vector3 target = new Vector3(x, y - 5, transform.position.z);
                    InstantiateBulletAndRotate(startPos, target);
                }
                break;
        }
        
    }

    private void InstantiateBulletAndRotate(Vector3 startPos, Vector3 target)
    {
        // Instantiate the bullet prefab
        GameObject projectile = Instantiate(bulletPrefab, startPos, transform.rotation);
        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            // Set the projectile's target and other properties
            enemyProjectile.PositionAndRotate(target);
        }
        else
        {
            Debug.LogError("Projectile does not have an EnemyProjectile component!");
        }
    }

    private void Die()
    {
        GameObject particles = Instantiate(deathParticles);
        particles.GetComponent<ParticleSystem>()?.Play();
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
