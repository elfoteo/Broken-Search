using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickuppableItem : MonoBehaviour
{
    public GameObject player;
    public float pickupRange = 2.0f; // Range within which the item can be picked up

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the player and the item
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within pickup range
        if (distanceToPlayer <= pickupRange)
        {
            // Check if the player presses the "E" key
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Call the player's Pickup method with the name of this item
                player.GetComponent<Player>().Pickup(gameObject.name);

                // Destroy this item
                Destroy(gameObject);
            }
        }
    }
}
