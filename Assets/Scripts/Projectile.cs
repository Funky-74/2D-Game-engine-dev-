using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rbody2D;

    void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rbody2D.AddForce(direction * force);
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

}
