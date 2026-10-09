using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Projectile : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        this.direction = direction;
        rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController p = gameObject.GetComponent<PlayerController>();

            if (!p.isInvin)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Vector2 pos = rb.transform.position;
                pos.x += other.gameObject.GetComponent<RectTransform>().sizeDelta.x * direction.x;
                rb.transform.position = pos;
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
