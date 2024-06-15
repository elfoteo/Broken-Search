using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed = 50.0f;
    private Camera mainCamera;
    public Rigidbody2D rb;
    public float damage;

    public void PositionAndRotate(Camera mainCamera, Vector3 target)
    {
        this.mainCamera = mainCamera;

        // Convert to world coordinates
        Vector3 targetPositionWorld = mainCamera.ScreenToWorldPoint(target);

        // Calculate the direction vector
        Vector3 direction = targetPositionWorld - transform.position;

        // Normalize the direction to get a unit vector
        direction.z = 0; // Ensure the z component is 0 for 2D physics
        direction.Normalize();

        // Apply the velocity to the Rigidbody2D
        rb.velocity = direction * speed;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
        // TODO: Spawn Effects
        Player playerComponent = other.gameObject.GetComponent<Player>();
        if (playerComponent != null) {
            playerComponent.Damage(damage);
        }
    }
}
