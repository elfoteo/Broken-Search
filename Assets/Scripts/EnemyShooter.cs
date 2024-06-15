using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [SerializeField] public float hoverHeightAnim = 0.75f; // Amplitude of the hover motion
    [SerializeField] public float hoverSpeed = 1.0f; // Speed of the hover motion

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Store the original position of the enemy
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeightAnim;

        // Update the position of the enemy
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
