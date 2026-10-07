using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool isHit = false;
    [SerializeField] float flyTime;
    private Rigidbody2D rb;
    private float elapsedTime;
    private int direction = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime < flyTime)
            {
                Vector2 pos = rb.transform.position;
                pos.y += Time.deltaTime * 2 * direction;
                rb.transform.position = pos;
            }
            else if (elapsedTime < flyTime * 2)
            {
                Vector2 pos = rb.transform.position;
                pos.y += Time.deltaTime * 2 * direction * -1;
                rb.transform.position = pos;
            }
            else
            {
                isHit = false;
                elapsedTime = 0;
            }
        }
    }

    public void Hit()
    {
        isHit = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1,0,0);
        
        
    }
}
