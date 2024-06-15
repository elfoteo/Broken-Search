using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [SerializeField] public Transform firePoint;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public int shootCooldown = 750;
    [SerializeField] public Camera mainCamera;
    public float lastShootTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && lastShootTime + shootCooldown < Time.time*1000)
        {
            Shoot();
            lastShootTime = Time.time*1000;
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        PlayerProjectile playerProjectile = projectile.GetComponent<PlayerProjectile>();
        if (playerProjectile != null)
        {
            playerProjectile.SetMainCamera(mainCamera);
        }
        else
        {
            Debug.LogError("Projectile does not have a PlayerProjectile component.");
        }
    }
}
