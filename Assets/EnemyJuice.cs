using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJuice : MonoBehaviour
{
    public float pickupRange = 2.0f; // Range within which the item can be picked up
    public float moveSpeed = 5.0f; // Speed at which the item moves towards the player
    public float pickupDistance = 0.5f; // Distance threshold for triggering pickup
    private GameObject player;

    private bool isMovingTowardsPlayer = false;

    void Start()
    {

    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the player and the item
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within pickup range
        if (distanceToPlayer <= pickupRange)
        {
            // Start moving towards the player
            isMovingTowardsPlayer = true;
        }

        // If moving towards player, lerp towards player's position
        if (isMovingTowardsPlayer)
        {
            Vector3 targetPosition = player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if close enough to trigger pickup
            if (Vector3.Distance(transform.position, targetPosition) <= pickupDistance)
            {
                // Call player's AddJuice method
                player.GetComponent<Player>().AddJuice(1);

                // Destroy this object
                Destroy(gameObject);
            }
        }
    }
}
