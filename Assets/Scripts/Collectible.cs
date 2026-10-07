using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private float floatTime = 0.5f;
    private float timePassed = 0;
    private int direction = 1;

    void Update()
    {
        Vector2 position = transform.position;

        position.y += direction * 0.5f * Time.deltaTime; // 0.5f controls speed its going up and down at
        transform.position = position;

        timePassed += Time.deltaTime;

        if (timePassed >= floatTime)
        {
            timePassed = 0;
            direction *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.AddPresent();
            Destroy(gameObject);
        }
    }
}