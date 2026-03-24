using UnityEngine;

public class Powerup : MonoBehaviour {
  // set in inspector
  public float speed;
    public AudioClip PickupClip;
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

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

            AudioSource.PlayClipAtPoint(PickupClip, Camera.main.transform.position, 1f);
        }
  }
}
