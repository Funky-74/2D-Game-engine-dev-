using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
    private Vector2 lastCheckpoint;
    //for invincibility
    private float invincibilityTime = 3.0f;
    private float invincibilityCount = 0f;
    public bool isInvin = false;
    private AudioSource audio;
    private bool isPlaying = false;
    [SerializeField] private AudioClip collectClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = jumpCountBase;
        lastCheckpoint = rb.transform.position;
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

        if(move.x!=0 && jumpCount == 0 && !isPlaying)
        {
            audio.Play();
            isPlaying = true;
        }
        if(isPlaying && (jumpCount>0 || move.x==0))
        {
            audio.Pause();
            isPlaying = false;
        }


        if(isInvin)
        {
            invincibilityCount += Time.deltaTime;
            if(invincibilityCount> invincibilityTime)
            {
                isInvin = false;
                invincibilityCount = 0;
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
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
        else if(collision.gameObject.tag=="EnemyProjectile" && !isInvin)
        {
            rb.linearVelocity = Vector2.zero;
            PlayerHit();
        }
        else if (collision.gameObject.tag == "EnemyProjectile" && isInvin)
        {
            rb.linearVelocity = Vector2.zero;
        }

    }

    private void PlayerHit()
    {
        lives--;
        uiManager.setLives(lives);
        //rb.transform.position = lastCheckpoint;
        if (lives == 0)
        {
            SceneManager.LoadScene("SampleScene");

        }
        isInvin = true;
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    public void AddPresent()
    {
        if(isPlaying)
        {
            audio.Pause();
        }

        audio.PlayOneShot(collectClip);
        if(isPlaying)
        {
            audio.Play();
        }
        presentsCollected++;
        if(score==4)
        {
            //create win scene which is then loaded here after the conition is met (made the same way as menu scene)
        }
        uiManager.setScore(presentsCollected);
    }


    public void reachCheckpoint()
    {
        lastCheckpoint = rb.transform.position;
    }
}