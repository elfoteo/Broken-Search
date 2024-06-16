using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private int shootCooldownMax = 3000; // Cooldown in milliseconds
    [SerializeField] private int shootCooldownMin = 2000; // Cooldown in milliseconds
    [SerializeField] private float shootingRange = 11.0f; // Range within which the enemy will shoot
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject teleportParticles;
    [SerializeField] private float gravityScale = 1.0f; // Gravity scale for the projectiles
    [SerializeField] private float landingTime = 1.0f; // Time for the projectiles to land
    [SerializeField] private int minTeleportPosition = -40;
    [SerializeField] private int maxTeleportPosition = 30;

    [Range(0, 100)] private float healAmount = 7;


    private float health;
    private int? shootCooldown;
    private static System.Random random = new System.Random();
    private float lastShootTime = 0f; // Last time the enemy shot
    private Vector3 originalPosition;

    private enum Attacks
    {
        PARABOLIC_BURST,
        TELEPORT
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update health bar
        bar.transform.localScale = new Vector3(health / maxHealth / 4, bar.transform.localScale.y, bar.transform.localScale.z);

        if (shootCooldown == null)
        {
            shootCooldown = random.Next(shootCooldownMin, shootCooldownMax);
        }

        // Check if the player is within range and it's time to shoot
        if (Time.time * 1000f - lastShootTime >= shootCooldown && IsPlayerInRange())
        {
            Attacks[] values = (Attacks[])Enum.GetValues(typeof(Attacks));
            Attacks chosenAttack = values[random.Next(values.Length)];
            ExecuteAttack(chosenAttack);
            lastShootTime = Time.time * 1000f; // Reset the last shoot time
            shootCooldown = null;
        }

        // Face the player
        FacePlayer();
    }

    private void FacePlayer()
    {
        Vector3 scale = transform.localScale;
        if (transform.position.x < player.transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    private bool IsPlayerInRange()
    {
        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= shootingRange;
    }

    private void ExecuteAttack(Attacks chosenAttack)
    {
        switch (chosenAttack)
        {
            case Attacks.PARABOLIC_BURST:
                ParabolicBurstAttack();
                break;
            case Attacks.TELEPORT:
                TeleportAttack();
                break;
        }
    }

    private void ParabolicBurstAttack()
    {
        int projectilesCount = 5;
        for (int i = 0; i < projectilesCount; i++)
        {
            float yOffset = (float)(random.NextDouble()*2D-1D); // Slightly offset each projectile
            Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
            InstantiateParabolicProjectile(target);
        }
    }

    private void InstantiateParabolicProjectile(Vector3 target)
    {
        GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = gravityScale;
            Vector2 toTarget = target - transform.position;
            Vector2 initialVelocity = new Vector2(
                toTarget.x / landingTime,
                (toTarget.y / landingTime) + 0.5f * Mathf.Abs(Physics2D.gravity.y) * gravityScale * landingTime
            );
            rb.velocity = initialVelocity;
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody2D component!");
        }
    }

    private void TeleportAttack()
    {
        // Instantiate particles at current position
        GameObject teleportParticles = Instantiate(this.teleportParticles, transform.position, Quaternion.identity);
        var particles = teleportParticles.GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }

        // Calculate new position for teleportation
        Vector3 newPosition = new Vector3(
            Mathf.Clamp(originalPosition.x + random.Next(minTeleportPosition, maxTeleportPosition + 1), minTeleportPosition, maxTeleportPosition),
            originalPosition.y,
            originalPosition.z);

        // Teleport to the new position
        transform.position = newPosition;

        // Heal n% of lost health
        float lostHealth = maxHealth - health;
        health += lostHealth * (healAmount / 100);
        health = Mathf.Min(health, maxHealth);

        // Destroy particles after 3 to 4 seconds
        Destroy(teleportParticles, UnityEngine.Random.Range(3f, 4f));
    }

    private void Die()
    {
        GameObject particles = Instantiate(deathParticles);
        particles.GetComponent<ParticleSystem>()?.Play();
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
