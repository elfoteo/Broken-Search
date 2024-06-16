using System.Collections;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float speed = 78;
    [SerializeField] private Camera mainCamera;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private float damage;
    
    public Animator animator;
    
    public void SetMainCamera(Camera mainCamera)
    {
        rb.gravityScale = 0;

        this.mainCamera = mainCamera;
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
        rb.velocity=Vector3.zero;
        

        animator.SetBool("Hit", true);

            
        // TODO: Spawn Effects
        // TODO: Damage enemies
        System.Type[] damageableTypes = { typeof(EnemyShooter), typeof(EnemyWall) };

        foreach (var type in damageableTypes)
        {
            var component = other.gameObject.GetComponent(type) as IDamageable;
            if (component != null)
            {
                Debug.Log(component);
                component.Damage(this.damage);
            }
        }

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}   