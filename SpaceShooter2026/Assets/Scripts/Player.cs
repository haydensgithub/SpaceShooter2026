using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Needed to grab the mouse position

public class Player : MonoBehaviour
{
    // set in inspector
    public float speed = 0.1f;
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform bulletSpawnPoint;
    public Slider sliderHealth;
    public GameObject expoPrefab;
    public UI ui;
    public AudioClip clipNormalFire1;
    public AudioClip clipNormalFire2;
    public AudioClip clipNoMissiles;
    public AudioClip clipSuperFire;
    public AudioClip clipHurt;
    public AudioClip clipPowerupReceived;
    public int MissileCount = 0;


    public static Player instance { get; private set; }

    // private fields
    private AudioSource audioSrc;
    private float health;
    private const float Y_LIMIT = 4.6f;
    private Rigidbody2D RBody;

    private void Start()
    {
        health = 1.0f;
        audioSrc = GetComponent<AudioSource>();
        RBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        instance = this;
    }
    void OnDisable()
    {
        instance = null;
    }

    private void Update()
    {
        sliderHealth.value = health;

        if (SpaceShooterInput.Instance.input.Fire.WasPressedThisFrame())
        {
            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
            int Rand = UnityEngine.Random.Range(0, 1);
            if (Rand == 0) audioSrc.clip = clipNormalFire1;
            else audioSrc.clip = clipNormalFire2;
            audioSrc.Play();
        }
        else if (SpaceShooterInput.Instance.input.SuperFire.WasPressedThisFrame())
        {
            if (MissileCount > 0)
            {
                MissileCount--;
                GameObject missileObject = Instantiate(missilePrefab, bulletSpawnPoint.position, transform.rotation);
                int Rand = UnityEngine.Random.Range(0, 1);
                if (Rand == 0) audioSrc.clip = clipNormalFire1;
                else audioSrc.clip = clipNormalFire2;
                audioSrc.Play();
            }
            else
            {
                {
                    audioSrc.clip = clipNoMissiles;
                    audioSrc.Play();
                }
            }
        }

        MovePlayer();
        RotatePlayer();

        if (this.transform.position.y > Y_LIMIT)
        {
            this.transform.position = new Vector3(transform.position.x, Y_LIMIT);
        }
        else if (this.transform.position.y < -Y_LIMIT)
        {
            this.transform.position = new Vector3(transform.position.x, -Y_LIMIT);
        }
    }

    public void DamageFromEnemy()
    {
        if (true)
        {
            audioSrc.clip = clipHurt;
            audioSrc.Play();
            health -= 0.25f;
            if (health <= 0)
            {
                var expoObj = Instantiate(expoPrefab, transform.position, Quaternion.identity);
                Destroy(expoObj, expoObj.GetComponent<ParticleSystem>().main.duration);
                Destroy(gameObject);
                ui.ShowGameOver();
            }
        }
    }

    public void MovePlayer()
    {
        // Assemble the movement vector
        var vertMove = SpaceShooterInput.Instance.input.MoveVertically.ReadValue<float>();
        var horMove = SpaceShooterInput.Instance.input.MoveHorizontally.ReadValue<float>();
        Vector3 MoveMerge = new Vector3(horMove, vertMove, 0);
        Vector3 Direction = MoveMerge.normalized;

        // Move at some speed in accordance with it
        transform.position += Direction * speed * Time.deltaTime;
    }

    public void RotatePlayer()
    {
        // Grab the mouse location
        Vector3 MouseScreen = Mouse.current.position.ReadValue();
        Vector3 MouseLocation = Camera.main.ScreenToWorldPoint(new Vector3(MouseScreen.x, MouseScreen.y, 0f));
        //MouseLocation.z = 0f; // Set z to 0 just in case

        // Get the vector facing the mouse
        Vector3 MouseDirection = MouseLocation - transform.position;

        // And do some rotator math to face mouse
        float MouseAngle = Mathf.Atan2(MouseDirection.y, MouseDirection.x) * Mathf.Rad2Deg;
        RBody.MoveRotation(MouseAngle);
    }
}
