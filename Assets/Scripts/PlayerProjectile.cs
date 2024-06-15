using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed = 50.0f;
    private Camera mainCamera;
    public Rigidbody2D rb;
    
    public void SetMainCamera(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the mouse position
        Vector3 mousePosition = Input.mousePosition;

        // Convert to world coordinates
        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);

        // Calculate the direction vector
        Vector3 direction = mousePositionWorld - transform.position;

        // Normalize the direction to get a unit vector
        direction.z = 0; // Ensure the z component is 0 for 2D physics
        direction.Normalize();

        // Apply the velocity to the Rigidbody2D
        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        // TODO: Spawn Effects
        // TODO: Damage enemies
    }
}
