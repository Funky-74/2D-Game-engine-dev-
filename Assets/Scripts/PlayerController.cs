using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    public int speed = 2;
    private Vector2 move;
    private Rigidbody2D rb;
    private int direction = 1;
    public int presentsCollected;

    private Animator animator;
    public float jumpHeight = 3;
    public int jumpCountBase = 2;
    private int jumpCount;
    private int score = 0;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private UIManager uiManager;
    private int lives = 3;
    private Vector2 lastPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = jumpCountBase;
        lastPosition = rb.position;
    }

    private void Fire()
    {
        GameObject go = Instantiate(projectilePrefab, rb.transform.position, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(-direction, 0), 600);
    }

    // Update is called once per frame
    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();

        if (InputSystem.actions["Jump"].IsPressed() && jumpCount > 0)
        {
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics2D.gravity.y * jumpHeight)), ForceMode2D.Impulse);
            jumpCount -= 1;
        }

        if (InputSystem.actions["Attack"].IsPressed())
        {
            InputSystem.actions["Attack"].Reset();
            Fire();
        }

        if (move.x != 0)
        {
            direction = move.x < 0 ? 1 : -1;
            animator.SetInteger("Direction", direction);
        }
        animator.SetFloat("Move", move.x);
    }

    void FixedUpdate()
    {
        Vector2 position = rb.transform.position;
        position.x += (move.x * speed) * Time.fixedDeltaTime;
        rb.transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = jumpCountBase;
        }
    }

    public void AddPresent()
    {
        presentsCollected++;
        uiManager.setScore(presentsCollected);
    }


    public void reachCheckpoint()
    {
        lastPosition = rb.transform.position;
    }
}