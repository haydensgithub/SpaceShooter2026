using UnityEngine;

public class Enemy : MonoBehaviour {
  // set in inspector
  public float speed;
  public GameObject expoPrefab;

  void Update() {
    transform.Translate(Vector3.left * speed * Time.deltaTime);
  }

  private void OnCollisionEnter2D(Collision2D c) {
    if (c.gameObject.CompareTag("Bullet")) {
      var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
      Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
      Destroy(gameObject);
      Destroy(c.gameObject);
      Score.Instance.HitEnemy();
      // Added for death timer
      DeathTimer.Instance.AddTime(2f);
    }
    else if (c.gameObject.CompareTag("Player")) {
      Destroy(gameObject);
      c.gameObject.GetComponent<Player>().DamageFromEnemy();
    }
  }
}
