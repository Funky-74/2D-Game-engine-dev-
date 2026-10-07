using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int state = 0;
    public int speed=5;
    public float JumpHeight = 3;
    public Boolean isJumping=false;
    public int presentsCollected;
    public int heartsCollected;
    private int lives = 3;
    private Vector2 lastPosition;



    public int direction = 1;


    [SerializeField]
    public GameObject projectilePrefab;

    [SerializeField]
    private UIManager uiManager;

    Animator animator;
    Rigidbody2D rigidbody2D;


    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        lastPosition = rigidbody2D.transform.position;

    }

    private void Fire()
    {
        GameObject go = Instantiate(projectilePrefab, rigidbody2D.transform.position, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 300);
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        float move = Input.GetAxis("Horizontal");
        position.x = position.x + speed * Time.deltaTime * move;
        transform.position = position;

        if (move != 0)
        {
            state = move < 0 ? -1 : 1;
            direction = state;
            animator.SetFloat("Move X", state);
            animator.SetFloat("Move Y", 1);
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;

            rigidbody2D.linearVelocity = Vector2.zero;

            rigidbody2D.AddForce(
                Vector2.up * Mathf.Sqrt(-2 * Physics2D.gravity.y * JumpHeight),
                ForceMode2D.Impulse
            );
        }

        //Projectile
        /*if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + new Vector2(direction, 0.2f), Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(new Vector2(direction, 0), 300);
        }*/



        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isJumping)
        {
            isJumping = false;
        }
        if(collision.gameObject.tag=="EnemyProjectile")
        {
            lives--;
            //gm.updateLives(lives);
            rigidbody2D.transform.position = lastPosition;
        }
            
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Checkpoint")
        {
            lastPosition = collision.transform.position;
            collision.gameObject.GetComponent<Checkpoint>().Hit();
        }
    }

    public void AddPresent()
    {
        presentsCollected++;
        //uiManager.setScore(presentsCollected);
    }

    public void AddHeart()
    {
        heartsCollected++;
    }
}
