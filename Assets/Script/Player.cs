using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    //config params
    [Header("Player")]
    [SerializeField] float moveSpeed = 5;
    [SerializeField] float padding = .5f;
    [SerializeField] int health = 500;
    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 1f;
    [SerializeField] AudioClip deathSfx;
    [SerializeField] AudioClip hurtSfx;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] [Range(0, 1)] float hitSoundVolume = 0.7f;

    //cached references
    Coroutine fireCoroutine;

    //game states
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBounadaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireCoroutine = StartCoroutine(FireContinously());
            
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);

        }
    }

    IEnumerator FireContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(hitSfx, Camera.main.transform.position, hitSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
        


    }

    private void SetUpMoveBounadaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
        AudioSource.PlayClipAtPoint(hurtSfx, Camera.main.transform.position, deathSoundVolume);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        damageDealer.Hit();
        health -= damageDealer.GetDamage();
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSfx, Camera.main.transform.position, deathSoundVolume);
            FindObjectOfType<SceneLoader>().LoadGameOver();
            
        }
    } 
    public int GetHitPoints()
    {
        return health;
    }


}
