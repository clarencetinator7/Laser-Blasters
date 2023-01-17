using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;
    [Header("Shooting")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] float projectileSpeed = 5f;
    
    [Header("Effects")]
    [SerializeField] AudioClip deathSfx;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] [Range(0, 1)] float hitSoundVolume = 0.7f;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] float durationOfExplosion = 2f;



    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    
    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(hitSfx, Camera.main.transform.position, hitSoundVolume);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);

    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        ParticleSystem explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSfx, Camera.main.transform.position, deathSoundVolume);
        
    }


 
}
