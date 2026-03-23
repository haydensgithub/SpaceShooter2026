using UnityEngine;

public class Missile : MonoBehaviour
{
    public float Speed = .95f;
    public float ExplosionRadius = 150;

    // Update is called once per frame
    void Update() {
        this.transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScreenOutOfBounds"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].CompareTag("Enemy"))
            {
                Enemy enemy = hits[i].GetComponent<Enemy>();

                if (enemy != null)
                {
                    // Missiles auto kill fast and tank enemies
                    enemy.TakeDamage(3);
                }
            }
        }

        Destroy(gameObject);
    }
}
