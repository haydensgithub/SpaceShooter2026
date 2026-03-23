using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // set in inspector
    public float Speed = 6f;
    public float Acceleration = 8f;
    public float RotationSpeed = 720f;
    public GameObject expoPrefab;

    private Vector2 CurrentVelocity;
    private Rigidbody2D RBody; 
   
    private void Awake()
    {
        RBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Player.instance == null) return;
        // Grab the vector facing the player, return if it is very very small (identical transforms)
        Vector2 PlayerDirection = (Vector2)(Player.instance.transform.position - transform.position);
        if (PlayerDirection.sqrMagnitude < 0.001) return;
        
        // The velocity we WANT to go in, will influence our real velocity
        Vector2 DesiredVelocity = PlayerDirection.normalized * Speed;

        CurrentVelocity = Vector2.MoveTowards(CurrentVelocity, DesiredVelocity, Acceleration * Time.fixedDeltaTime);

        RBody.linearVelocity = CurrentVelocity;

        // Rotate to face the player
        float RotationAngle = Mathf.Atan2(-PlayerDirection.y, -PlayerDirection.x) * Mathf.Rad2Deg;
        RBody.MoveRotation(RotationAngle);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.CompareTag("Bullet"))
        {
            Die(c);
        }
        else if (c.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            c.gameObject.GetComponent<Player>().DamageFromEnemy();
        }
    }

    public void Die(Collider2D c)
    {
        var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
        Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
        Destroy(c.gameObject);
        Score.Instance.HitEnemy();
        // Added for death timer
        DeathTimer.Instance.AddTime(2f);
    }
}
