using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;

    public Transform target;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        targetPosition.z = transform.position.z;

        
        Vector3 clamped = new Vector3(transform.position.x, Mathf.Min(Mathf.Max(transform.position.y, -0.9f), 0.9f), transform.position.z);
        transform.position = Vector3.SmoothDamp(clamped, targetPosition, ref velocity, damping);
    }
}
