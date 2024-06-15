using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFirepoint : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private Camera mainCamera; // Reference to the main camera
    [SerializeField] private float distanceFromPlayer = 1.0f; // Distance of the firepoint from the player

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to world coordinates
        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);
        mousePositionWorld.z = 0; // Ensure the z component is 0 for 2D

        // Calculate the direction from the player to the mouse position
        Vector3 direction = mousePositionWorld - player.transform.position;
        direction.Normalize();

        // Calculate the new position for the firepoint
        Vector3 firepointPosition = player.transform.position + direction * distanceFromPlayer;

        // Set the firepoint's position and rotation
        transform.position = firepointPosition;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
