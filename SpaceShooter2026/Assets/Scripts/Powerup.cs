using UnityEngine;

public class Powerup : MonoBehaviour {
  // set in inspector
  public float speed;

  void Update() {
    transform.Translate(Vector3.left * speed * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D c) {
    if (c.gameObject.CompareTag("Bullet")) {
      Destroy(gameObject);
      Destroy(c.gameObject);
    }
    else if (c.gameObject.CompareTag("Player")) {
           Destroy(gameObject);
            Player p = c.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.MissileCount++;
            }
        }
  }
}
